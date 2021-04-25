using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MediaExplorer.Core.Models
{
    public class MediaCharacter
    {
        public string Name { get; private set; }

        private ObservableCollection<string> _tags;
        public ReadOnlyObservableCollection<string> Tags { get { return new ReadOnlyObservableCollection<string>(_tags); } }

        public MediaCharacter()
        {
            Name = string.Empty;
            _tags = new ObservableCollection<string>();
        }
    }
}
