using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class VirtualAlbumFile : VirtualFileSystemObject
    {
        private string _realPath;

        [field: NonSerialized]
        public Album Album { get; set; }

        public VirtualAlbumFile(Album album, VirtualFolder parent) : base(album.Name, parent)
        {
            Album = album;
            _realPath = Album.FilePath;
        }

        public override void InitializeNonSerializedMembers()
        {
           // _album = Album.FromFilePath(_realPath);
        }
    }
}
