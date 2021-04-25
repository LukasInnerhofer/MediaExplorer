using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class Media
    {
        public string Path { get; private set; }
        public MediaMetadata Metadata { get; private set; }

        private Media(string path, MediaMetadata metadata)
        {
            Path = path;
            Metadata = metadata;
        }

        public Media(string path) : this(path, new MediaMetadata())
        {

        }

        public Media() : this(string.Empty, new MediaMetadata())
        {

        }
    }
}
