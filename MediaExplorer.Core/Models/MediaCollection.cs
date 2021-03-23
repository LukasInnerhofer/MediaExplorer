using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaCollection
    {
        private List<Media> _media;
        public IReadOnlyCollection<Media> Media { get { return _media; } }

        private List<MediaMetadata> _metadata;
        public IReadOnlyCollection<MediaMetadata> Metadata { get { return _metadata; } }

        public MediaCollection()
        {
            _media = new List<Media>();
            _metadata = new List<MediaMetadata>();
        }
    }
}
