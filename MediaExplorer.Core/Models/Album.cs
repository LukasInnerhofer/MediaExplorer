﻿using MediaExplorer.Core.Services;
using MvvmCross;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class Album
    {
        [NonSerialized]
        private byte[] _key;
        public byte[] Key { get { return _key; } }

        public string Name { get; private set; }
        
        [field: NonSerialized]
        public string FilePath { get; private set; }

        private string _basePath;

        private ObservableCollection<MediaCollection> _mediaCollections;
        public ReadOnlyObservableCollection<MediaCollection> MediaCollections { get { return new ReadOnlyObservableCollection<MediaCollection>(_mediaCollections); } }

        [field: NonSerialized]
        private ObservableCollection<string> _allTags;
        public ReadOnlyObservableCollection<string> AllTags { get { return new ReadOnlyObservableCollection<string>(_allTags); } }

        [field: NonSerialized]
        private ObservableCollection<string> _allCharacterTags;
        public ReadOnlyObservableCollection<string> AllCharacterTags { get { return new ReadOnlyObservableCollection<string>(_allCharacterTags); } }

        [field: NonSerialized]
        private ObservableCollection<string> _allCharacterNames;
        public ReadOnlyObservableCollection<string> AllCharacterNames { get { return new ReadOnlyObservableCollection<string>(_allCharacterNames); } }

        public Album()
        {
            Name = string.Empty;
            FilePath = string.Empty;
            _mediaCollections = new ObservableCollection<MediaCollection>();
            _allTags = new ObservableCollection<string>();
            _allCharacterTags = new ObservableCollection<string>();
            _allCharacterNames = new ObservableCollection<string>();
        }

        public static async Task<Album> FromBasePathAsync(string basePath, byte[] key)
        {
            var album = new Album();
            album._key = key;

            album.Name = basePath.Split(Path.DirectorySeparatorChar).Last();
            album._basePath = basePath + Path.DirectorySeparatorChar + ".mediaexplorer";
            Directory.CreateDirectory(album._basePath);
            album.FindEncryptedMedia();
            await album.FindMediaAsync(basePath);
            await album.FindMediaCollectionsAsync(basePath);

            album.FilePath = album._basePath + Path.DirectorySeparatorChar + "album";
            using (var fs = new FileStream(album.FilePath, FileMode.Create))
            {
                await Mvx.IoCProvider.Resolve<ICryptographyService>().SerializeAsync(fs, album, album._key);
            }

            album.CollectMetadata();

            return album;
        }

        public void InitializeNonSerializedMembers(byte[] key, string filePath)
        {
            _key = key;
            FilePath = filePath;
            if(filePath.Replace(Path.DirectorySeparatorChar + "album", "") != _basePath)
            {
                string oldBasePath = _basePath;
                _basePath = FilePath.Replace(Path.DirectorySeparatorChar + "album", "");
                foreach (MediaCollection collection in _mediaCollections)
                {
                    foreach (Media media in collection.Media)
                    {
                        media.UpdatePath(media.Path.Replace(oldBasePath, _basePath));
                    }
                }
            }
            CollectMetadata();
        }

        public async Task AddMedia(List<Tuple<string, MemoryStream>> streams)
        {
            foreach(var source in streams)
            {
                string hash = BitConverter.ToString(await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(source.Item2)).Replace("-", "");
                source.Item2.Seek(0, SeekOrigin.Begin);
                string fileName = _basePath + Path.DirectorySeparatorChar + "media" + Path.DirectorySeparatorChar + hash + "." + source.Item1;
                using(var fs = new FileStream(fileName, FileMode.Create))
                {
                    await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(source.Item2, fs, _key);
                }
                _mediaCollections.Add(new MediaCollection(hash, new Media(fileName)));
            }
        }

        public async Task AddMediaCollection(List<Tuple<string, MemoryStream>> streams)
        {
            var files = new List<string>();
            byte[] fileHash;
            string encryptedDirPath;
            string encryptedFilePath;

            string collectionName = string.Empty;
            encryptedDirPath = _basePath + Path.DirectorySeparatorChar + "media" + Path.DirectorySeparatorChar + "temp";
            Directory.CreateDirectory(encryptedDirPath);
            using (var collectionHashStream = new MemoryStream())
            {
                foreach (Tuple<string, MemoryStream> stream in streams)
                {
                    encryptedFilePath = encryptedDirPath;

                    fileHash = await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(stream.Item2);
                    stream.Item2.Seek(0, SeekOrigin.Begin);
                    collectionHashStream.Write(fileHash, 0, fileHash.Length);

                    encryptedFilePath += Path.DirectorySeparatorChar +
                        BitConverter.ToString(fileHash).Replace("-", "") + "." + stream.Item1;

                    using (var dest = new FileStream(encryptedFilePath, FileMode.Create))
                    {
                        await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(stream.Item2, dest, _key);
                    }

                    files.Add(encryptedFilePath);
                }

                collectionName = BitConverter.ToString(await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(collectionHashStream)).Replace("-", "");
                Directory.Move(encryptedDirPath, Path.GetDirectoryName(encryptedDirPath) + Path.DirectorySeparatorChar + collectionName);
            }
            if (files.Count > 0)
            {
                List<Media> media = new List<Media>();
                foreach (string file in files)
                {
                    media.Add(new Media(file.Replace("temp", collectionName)));
                }
                _mediaCollections.Add(new MediaCollection(collectionName, media));
            }
        }

        public async Task AddMedia(List<string> files)
        {
            foreach(string file in files)
            {
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    string hash = BitConverter.ToString(await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(fs)).Replace("-", "");
                    fs.Seek(0, SeekOrigin.Begin);
                    string fileName = _basePath + Path.DirectorySeparatorChar + "media" + Path.DirectorySeparatorChar + hash + "." + file.Split('.').Last();
                    using (var outStream = new FileStream(fileName, FileMode.Create))
                    {
                        await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(fs, outStream, _key);
                    }
                    _mediaCollections.Add(new MediaCollection(hash, new Media(fileName)));
                }
            }
        }

        public async Task SaveAsync()
        {
            using(var fs = new FileStream(FilePath, FileMode.Create))
            {
                await Mvx.IoCProvider.Resolve<ICryptographyService>().SerializeAsync(fs, this, Key);
            }
        }

        private void CollectMetadata()
        {
            _allTags = new ObservableCollection<string>();
            _allCharacterTags = new ObservableCollection<string>();
            _allCharacterNames = new ObservableCollection<string>();
            foreach (MediaCollection collection in _mediaCollections)
            {
                foreach(Media media in collection.Media)
                {
                    foreach(MediaTag tag in media.Metadata.Tags)
                    {
                        if(!_allTags.Contains(tag.Text))
                        {
                            _allTags.Add(tag.Text);
                        }
                    }
                    foreach(MediaCharacter character in media.Metadata.Characters)
                    {
                        foreach(MediaTag tag in character.Tags)
                        {
                            if(!_allCharacterTags.Contains(tag.Text))
                            {
                                _allCharacterTags.Add(tag.Text);
                            }
                        }
                        if(!_allCharacterNames.Contains(character.Name))
                        {
                            _allCharacterNames.Add(character.Name);
                        }
                        ((INotifyCollectionChanged)character.Tags).CollectionChanged += MediaCharacterTagsChanged;
                    }
                    ((INotifyCollectionChanged)media.Metadata.Characters).CollectionChanged += MediaCharactersChanged;
                    ((INotifyCollectionChanged)media.Metadata.Tags).CollectionChanged += MediaTagsChanged;
                }
                ((INotifyCollectionChanged)collection.Media).CollectionChanged += MediaChanged;
            }
            ((INotifyCollectionChanged)_mediaCollections).CollectionChanged += MediaCollectionsChanged;
        }

        private void MediaCharactersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MediaCharacter character in e.NewItems)
                {
                    foreach(MediaTag tag in character.Tags)
                    {
                        if (!_allCharacterTags.Contains(tag.Text))
                        {
                            _allCharacterTags.Add(tag.Text);
                        }
                    }
                    if(!_allCharacterNames.Contains(character.Name))
                    {
                        _allCharacterNames.Add(character.Name);
                    }
                    ((INotifyCollectionChanged)character.Tags).CollectionChanged += MediaCharacterTagsChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (MediaCharacter oldCharacter in e.OldItems)
                {
                    ReadOnlyObservableCollection<MediaTag> oldTags = oldCharacter.Tags;
                    bool removeCharacterName = true;

                    foreach (MediaCollection collection in _mediaCollections)
                    {
                        foreach (Media media in collection.Media)
                        {
                            foreach (MediaCharacter character in media.Metadata.Characters)
                            {
                                if (character.Name == oldCharacter.Name)
                                {
                                    removeCharacterName = false;
                                }
                            }
                        }
                    }

                    foreach (MediaTag oldTag in oldTags)
                    {
                        bool removeTag = true;
                        foreach (MediaCollection collection in _mediaCollections)
                        {
                            foreach (Media media in collection.Media)
                            {
                                foreach(MediaCharacter character in media.Metadata.Characters)
                                {
                                    if(character.Name == oldCharacter.Name)
                                    {
                                        removeCharacterName = false;
                                    }

                                    foreach (MediaTag tag in character.Tags)
                                    {
                                        if (tag.Text == oldTag.Text)
                                        {
                                            removeTag = false;
                                        }
                                    }
                                }
                            }
                        }
                        if (removeTag)
                        {
                            _allCharacterTags.Remove(oldTag.Text);
                        }
                    }
                    if(removeCharacterName)
                    {
                        _allCharacterNames.Remove(oldCharacter.Name);
                    }
                }
            }
        }

        private void MediaCollectionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MediaCollection collection in e.NewItems)
                {
                    foreach(Media media in collection.Media)
                    {
                        ((INotifyCollectionChanged)media.Metadata.Tags).CollectionChanged += MediaTagsChanged;
                        foreach(MediaCharacter character in media.Metadata.Characters)
                        {
                            ((INotifyCollectionChanged)character.Tags).CollectionChanged += MediaCharacterTagsChanged;
                        }
                    }
                }
            }
        }

        private void MediaCharacterTagsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MediaTag tag in e.NewItems)
                {
                    if(!_allCharacterTags.Contains(tag.Text))
                    {
                        _allCharacterTags.Add(tag.Text);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (MediaTag oldTag in e.OldItems)
                {
                    bool remove = true;
                    foreach (MediaCollection collection in _mediaCollections)
                    {
                        foreach (Media media in collection.Media)
                        {
                            foreach (MediaCharacter character in media.Metadata.Characters)
                            {
                                foreach(MediaTag tag in character.Tags)
                                {
                                    if (tag.Text == oldTag.Text)
                                    {
                                        remove = false;
                                    }
                                }
                            }
                        }
                    }
                    if (remove)
                    {
                        _allCharacterTags.Remove(oldTag.Text);
                    }
                }
            }
        }

        private void MediaChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Media media in e.NewItems)
                {
                    ((INotifyCollectionChanged)media.Metadata.Tags).CollectionChanged += MediaTagsChanged;
                    foreach (MediaCharacter character in media.Metadata.Characters)
                    {
                        ((INotifyCollectionChanged)character.Tags).CollectionChanged += MediaCharacterTagsChanged;
                    }
                }
            }
        }

        private void MediaTagsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(MediaTag newTag in e.NewItems)
                {
                    if (!_allTags.Contains(newTag.Text))
                    {
                        _allTags.Add(newTag.Text);
                    }
                }
            }
            if (e.OldItems != null)
            {
                foreach (MediaTag oldTag in e.OldItems)
                {
                    bool remove = true;
                    foreach (MediaCollection collection in _mediaCollections)
                    {
                        foreach (Media media in collection.Media)
                        {
                            foreach (MediaTag tag in media.Metadata.Tags)
                            {
                                if(tag.Text == oldTag.Text)
                                {
                                    remove = false;
                                }
                            }
                        }
                    }
                    if(remove)
                    {
                        _allTags.Remove(oldTag.Text);
                    }
                }
            }
        }

        private async Task FindMediaAsync(string path)
        {
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + ".mediaexplorer" + Path.DirectorySeparatorChar + "media");
            string encryptedPath = string.Empty;
            foreach (string file in Directory.EnumerateFiles(path))
            {
                string mediaName = string.Empty;
                using (var src = new FileStream(file, FileMode.Open))
                {
                    mediaName = BitConverter.ToString(
                            await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(src)).Replace("-", "") + "." +
                        Path.GetFileName(file).Split('.').Last();
                    encryptedPath = _basePath + Path.DirectorySeparatorChar +
                        "media" + Path.DirectorySeparatorChar +
                        mediaName;
                    src.Seek(0, SeekOrigin.Begin);
                    using (var dest = new FileStream(encryptedPath, FileMode.Create))
                    {
                        await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(src, dest, _key);
                    }
                }
                _mediaCollections.Add(new MediaCollection(mediaName.Split('.').First(), new Media(encryptedPath)));
            }
        }

        private async Task FindMediaCollectionsAsync(string path)
        {
            var files = new List<string>();
            byte[] fileHash;
            string encryptedDirPath;
            string encryptedFilePath;

            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                string collectionName = string.Empty;
                encryptedDirPath = _basePath + Path.DirectorySeparatorChar +
                    "media" + Path.DirectorySeparatorChar +
                    folder.Split(Path.DirectorySeparatorChar).Last();
                using (var folderHashStream = new MemoryStream())
                {
                    if (folder.Split(Path.DirectorySeparatorChar).Last() == ".mediaexplorer") continue;
                    foreach (string file in Directory.EnumerateFiles(folder))
                    {
                        Directory.CreateDirectory(encryptedDirPath);
                        encryptedFilePath = encryptedDirPath;

                        using (var src = new FileStream(file, FileMode.Open))
                        {
                            fileHash = await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(src);
                            src.Seek(0, SeekOrigin.Begin);
                            folderHashStream.Write(fileHash, 0, fileHash.Length);

                            encryptedFilePath += Path.DirectorySeparatorChar +
                                BitConverter.ToString(fileHash).Replace("-", "") + "." +
                                Path.GetFileName(file).Split('.').Last();

                            using (var dest = new FileStream(encryptedFilePath, FileMode.Create))
                            {
                                await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(src, dest, _key);
                            }
                        }

                        files.Add(encryptedFilePath);
                    }

                    collectionName = BitConverter.ToString(
                                await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(folderHashStream)).Replace("-", "");
                    Directory.Move(encryptedDirPath, Path.GetDirectoryName(encryptedDirPath) + Path.DirectorySeparatorChar +
                            collectionName);
                }
                if (files.Count > 0)
                {
                    List<Media> media = new List<Media>();
                    foreach(string file in files)
                    {
                        media.Add(new Media(file.Replace(folder.Split(Path.DirectorySeparatorChar).Last(), collectionName)));
                    }
                    _mediaCollections.Add(new MediaCollection(collectionName, media));
                    files.Clear();
                }
            }
        }

        private void FindEncryptedMedia()
        {
            foreach (string folder in Directory.EnumerateDirectories($"{_basePath}{Path.DirectorySeparatorChar}media"))
            {
                var media = new List<Media>();
                foreach (string file in Directory.EnumerateFiles(folder))
                {
                    media.Add(new Media(file));
                }
                if(media.Count > 0)
                {
                    _mediaCollections.Add(new MediaCollection(folder.Split(Path.DirectorySeparatorChar).Last(), media));
                }
            }
            foreach (string file in Directory.EnumerateFiles($"{_basePath}{Path.DirectorySeparatorChar}media"))
            {
                _mediaCollections.Add(new MediaCollection(Path.GetFileNameWithoutExtension(file), new Media(file)));
            }
        }
    }
}
