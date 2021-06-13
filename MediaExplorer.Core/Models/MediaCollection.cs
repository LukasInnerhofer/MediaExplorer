using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaCollection
    {
        private string _name;
        public string Name { get { return _name; } }

        private ObservableCollection<Media> _media;
        public ReadOnlyObservableCollection<Media> Media { get { return new ReadOnlyObservableCollection<Media>(_media); } }

        public MediaCollection(string name, List<Media> media)
        {
            _name = name;
            _media = new ObservableCollection<Media>(media);
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
