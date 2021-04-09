using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class Media
    {
        public string Path { get; private set; }

        public Media()
        {
            Path = string.Empty;
        }

        public Media(string path)
        {
            Path = path;
        }
    }
}
