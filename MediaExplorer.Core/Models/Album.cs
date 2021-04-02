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

            album.Name = basePath.Split(Path.DirectorySeparatorChar).Last();
            album._basePath = basePath + Path.DirectorySeparatorChar + ".mediaexplorer";
            Directory.CreateDirectory(album._basePath);
            await album.FindMediaAsync(basePath);
            await album.FindMediaCollectionsAsync(basePath);

            return album;
        }

        private async Task FindMediaAsync(string path)
        {
            Directory.CreateDirectory(path + Path.DirectorySeparatorChar + ".mediaexplorer" + Path.DirectorySeparatorChar + "media");
            string encryptedPath = string.Empty;
            foreach (string file in Directory.EnumerateFiles(path))
            {
                using (var src = new FileStream(file, FileMode.Open))
                {
                    encryptedPath = _basePath + Path.DirectorySeparatorChar +
                        "media" + Path.DirectorySeparatorChar +
                        BitConverter.ToString(
                            await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(src)).Replace("-", "") + "." +
                        Path.GetFileName(file).Split('.').Last();
                    src.Seek(0, SeekOrigin.Begin);
                    using (var dest = new FileStream(encryptedPath, FileMode.Create))
                    {
                        await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(src, dest, _key);
                    }
                }
                _mediaCollections.Add(new MediaCollection(new Media(encryptedPath)));
            }
        }

        private async Task FindMediaCollectionsAsync(string path)
        {
            var media = new List<Media>();
            byte[] fileHash;
            string encryptedDirPath;
            string encryptedFilePath;

            foreach (string folder in Directory.EnumerateDirectories(path))
            {
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
                            folderHashStream.Write(fileHash, 0, fileHash.Length);

                            encryptedFilePath += Path.DirectorySeparatorChar +
                                BitConverter.ToString(fileHash).Replace("-", "") + "." +
                                Path.GetFileName(file).Split('.').Last();

                            using (var dest = new FileStream(encryptedFilePath, FileMode.Create))
                            {
                                await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(src, dest, _key);
                            }
                        }

                        media.Add(new Media(encryptedFilePath));
                    }

                    Directory.Move(encryptedDirPath, Path.GetDirectoryName(encryptedDirPath) + Path.DirectorySeparatorChar +
                            BitConverter.ToString(
                                await Mvx.IoCProvider.Resolve<ICryptographyService>().ComputeHashAsync(folderHashStream)).Replace("-", ""));
                }
            }
            if (media.Count > 0)
            {
                _mediaCollections.Add(new MediaCollection(media));
            }
        }
    }
}
