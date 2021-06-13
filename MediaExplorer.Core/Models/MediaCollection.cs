using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaCollection : ISerializable
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

        public MediaCollection(SerializationInfo info, StreamingContext context)
        {
            var enumerator = info.GetEnumerator();
            while(enumerator.MoveNext())
            {
                var current = enumerator.Current;
                Debug.WriteLine(string.Format("{0} of type {1}: {2}", current.Name, current.ObjectType, current.Value));
                if (current.Name == "Media" && current.ObjectType == typeof(ObservableCollection<Media>))
                {
                    _media = (ObservableCollection<Media>)current.Value;
                }
                else if (current.Name == "_media" && current.ObjectType == typeof(List<Media>))
                {
                    var old = (List<Media>)current.Value;
                    _media = new ObservableCollection<Media>(old);
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Media", Media);
        }
    }
}
