using MediaExplorer.Core.Services;
using MvvmCross;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class VirtualAlbumFile : VirtualFileSystemObject
    {
        private string _realPath;

        [NonSerialized]
        byte[] _key;

        [field: NonSerialized]
        public Album Album { get; set; }

        public VirtualAlbumFile(Album album, VirtualFolder parent) : base(album.Name, parent)
        {
            Album = album;
            _realPath = Album.FilePath;
            _key = Album.Key;
        }

        public override async Task InitializeNonSerializedMembers(object param)
        {
            _key = param as byte[];
            using(var fs = new FileStream(_realPath, FileMode.Open))
            {
                Album = await Mvx.IoCProvider.Resolve<ICryptographyService>().DeserializeAsync<Album>(fs, _key);
                Album.InitializeNonSerializedMembers(_key, _realPath);
            }
        }
    }
}
