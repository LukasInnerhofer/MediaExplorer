using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaMetadata
    {
        private ObservableCollection<MediaTag> _tags;
        public ReadOnlyObservableCollection<MediaTag> Tags { get { return new ReadOnlyObservableCollection<MediaTag>(_tags); } }

        private ObservableCollection<MediaCharacter> _characters;
        public ReadOnlyObservableCollection<MediaCharacter> Characters { get { return new ReadOnlyObservableCollection<MediaCharacter>(_characters); } }

        public MediaMetadata()
        {
            _tags = new ObservableCollection<MediaTag>();
            _characters = new ObservableCollection<MediaCharacter>();
        }

        public void AddTag(string text)
        {
            if(!_tags.Any(x => x.Text == text))
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
