using MediaExplorer.Core.Services;
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

        private Album()
        {
            Name = string.Empty;
            FilePath = string.Empty;
            _mediaCollections = new ObservableCollection<MediaCollection>();
        }

        public static async Task<Album> FromBasePathAsync(string basePath, byte[] key)
        {
            var album = new Album();
            album._key = key;

            album.Name = basePath.Split(Path.DirectorySeparatorChar).Last();
            album._basePath = basePath + Path.DirectorySeparatorChar + ".mediaexplorer";
            Directory.CreateDirectory(album._basePath);
            await album.FindMediaAsync(basePath);
            await album.FindMediaCollectionsAsync(basePath);

            album.FilePath = album._basePath + Path.DirectorySeparatorChar + "album";
            using (var fs = new FileStream(album.FilePath, FileMode.Create))
            {
                await Mvx.IoCProvider.Resolve<ICryptographyService>().SerializeAsync(fs, album, album._key);
            }

            return album;
        }

        public void InitializeNonSerializedMembers(byte[] key, string filePath)
        {
            _key = key;
            FilePath = filePath;
        }

        public async Task AddMedia(List<KeyValuePair<string, MemoryStream>> streams)
        {
            foreach(var source in streams)
            {
                string hash = BitConverter.ToString(await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(source.Value)).Replace("-", "");
                source.Value.Seek(0, SeekOrigin.Begin);
                string fileName = _basePath + Path.DirectorySeparatorChar + "media" + Path.DirectorySeparatorChar + hash + "." + source.Key;
                using(var fs = new FileStream(fileName, FileMode.Create))
                {
                    await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(source.Value, fs, _key);
                }
                _mediaCollections.Add(new MediaCollection(hash, new Media(fileName)));
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
    }
}
