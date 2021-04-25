using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaCharacter
    {
        public string Name { get; private set; }

        private ObservableCollection<MediaTag> _tags;
        public ReadOnlyObservableCollection<MediaTag> Tags { get { return new ReadOnlyObservableCollection<MediaTag>(_tags); } }

        public MediaCharacter(string name)
        {
            Name = name;
            _tags = new ObservableCollection<MediaTag>();
        }

        public MediaCharacter() : this(string.Empty)
        {
            
        }

        public void AddTag(string text)
        {
            if (!_tags.Any(x => x.Text == text))
            {
                _tags.Add(new MediaTag(text));
            }
        }

        public void RemoveTag(string text)
        {
            _tags.Remove(_tags.First(x => x.Text == text));
        }
    }
}
