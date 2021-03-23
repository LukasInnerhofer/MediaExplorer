using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaMetadata
    {
        private List<string> _tags;
        public IReadOnlyCollection<string> Tags { get { return _tags; } }

        MediaMetadata()
        {
            _tags = new List<string>();
        }
    }
}
