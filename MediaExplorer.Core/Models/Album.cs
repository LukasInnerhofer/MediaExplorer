using MediaExplorer.Core.Services;
using MvvmCross;
using System;
using System.Collections.Generic;
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

        public string Name { get; private set; }
        
        [field: NonSerialized]
        public string FilePath { get; private set; }

        private string _basePath { get; set; }

        private List<MediaCollection> _mediaCollections;
        public IReadOnlyCollection<MediaCollection> MediaCollections { get { return _mediaCollections; } }

        public Album()
        {
            Name = string.Empty;
            FilePath = string.Empty;
            _mediaCollections = new List<MediaCollection>();
        }

        public static async Task<Album> FromBasePathAsync(string basePath, byte[] key)
        {
            var album = new Album();
            album._key = key;

            Directory.CreateDirectory(basePath + Path.DirectorySeparatorChar + ".mediaexplorer");
            album.Name = basePath.Split(Path.DirectorySeparatorChar).Last();
            album._basePath = basePath + Path.DirectorySeparatorChar + ".mediaexplorer";
            await album.FindMediaAsync(basePath);
            await album.FindMediaCollectionsAsync(basePath);

            return album;
        }

        private async Task FindMediaAsync(string path)
        {
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + ".mediaexplorer" + Path.DirectorySeparatorChar + "media");
            foreach (string file in Directory.EnumerateFiles(path))
            {
                string encryptedPath = _basePath + Path.DirectorySeparatorChar +
                    "media" + Path.DirectorySeparatorChar +
                    Path.GetFileName(file);
                using (var src = new FileStream(file, FileMode.Open))
                {
                    using (var dest = new FileStream(encryptedPath, FileMode.Create))
                    {
                        await Mvx.IoCProvider.Resolve<ICryptographyService>().Encrypt(src, dest, _key);
                    }
                }
                _mediaCollections.Add(new MediaCollection(new Media(encryptedPath)));
            }
        }

        private async Task FindMediaCollectionsAsync(string path)
        {
            var media = new List<Media>();
            foreach (string folder in Directory.EnumerateDirectories(path))
            {
                if (folder.Split(Path.DirectorySeparatorChar).Last() == ".mediaexplorer") continue;
                foreach (string file in Directory.EnumerateFiles(folder))
                {
                    string encryptedPath = _basePath + Path.DirectorySeparatorChar +
                        "media" + Path.DirectorySeparatorChar +
                        folder.Split(Path.DirectorySeparatorChar).Last();
                    Directory.CreateDirectory(encryptedPath);
                    encryptedPath += Path.DirectorySeparatorChar + Path.GetFileName(file);
                    using (var src = new FileStream(file, FileMode.Open))
                    {
                        using (var dest = new FileStream(encryptedPath, FileMode.Create))
                        {
                            await Mvx.IoCProvider.Resolve<ICryptographyService>().Encrypt(src, dest, _key);
                        }
                    }
                    media.Add(new Media(encryptedPath));
                }
            }
            if (media.Count > 0)
            {
                _mediaCollections.Add(new MediaCollection(media));
            }
        }
    }
}
