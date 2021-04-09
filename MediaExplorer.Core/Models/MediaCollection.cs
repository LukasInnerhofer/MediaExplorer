using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaCollection
    {
        private string _name;
        public string Name { get { return _name; } }

        private List<Media> _media;
        public IReadOnlyList<Media> Media { get { return _media; } }

        private List<MediaMetadata> _metadata;
        public IReadOnlyCollection<MediaMetadata> Metadata { get { return _metadata; } }

        private MediaCollection(string name, List<Media> media, List<MediaMetadata> metadata)
        {
            _name = name;
            _media = media;
            _metadata = metadata;
        }

        public MediaCollection() : this(string.Empty, new List<Media>(), new List<MediaMetadata>())
        {

        }

        public MediaCollection(string name, Media media) : this(name, new List<Media>(), new List<MediaMetadata>())
        {
            _media.Add(media);
        }

        public MediaCollection(string name, List<Media> media) : this(name, media, new List<MediaMetadata>())
        {

        }
    }
}
