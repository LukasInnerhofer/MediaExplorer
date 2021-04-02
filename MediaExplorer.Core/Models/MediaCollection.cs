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

        private MediaCollection(List<Media> media, List<MediaMetadata> metadata)
        {
            _media = media;
            _metadata = metadata;
        }

        public MediaCollection() : this(new List<Media>(), new List<MediaMetadata>())
        {

        }

        public MediaCollection(Media media) : this()
        {
            _media.Add(media);
        }

        public MediaCollection(List<Media> media) : this(media, new List<MediaMetadata>())
        {

        }
    }
}
