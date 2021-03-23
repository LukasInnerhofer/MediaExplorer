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

        [NonSerialized]
        private Album _album;

        public VirtualAlbumFile(Album album, VirtualFolder parent) : base(album.Name, parent)
        {
            _album = album;
            _realPath = _album.FilePath;
        }

        public override void InitializeNonSerializedMembers()
        {
           // _album = Album.FromFilePath(_realPath);
        }
    }
}
