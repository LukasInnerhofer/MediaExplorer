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

        public Media(string path)
        {
            Path = path;
        }

        public Media() : this(string.Empty)
        {
            Metadata = new MediaMetadata();
        }
    }
}
