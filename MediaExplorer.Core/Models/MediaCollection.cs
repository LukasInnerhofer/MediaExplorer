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

        public MediaCollection(string name, List<Media> media)
        {
            _name = name;
            _media = media;
        }

        public MediaCollection() : this(string.Empty, new List<Media>())
        {

        }

        public MediaCollection(string name, Media media) : this(name, new List<Media>())
        {
            _media.Add(media);
        }
    }
}
