﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaMetadata
    {
        private ObservableCollection<string> _tags;
        public ReadOnlyObservableCollection<string> Tags { get { return new ReadOnlyObservableCollection<string>(_tags); } }

        private ObservableCollection<MediaCharacter> _characters;
        public ReadOnlyObservableCollection<MediaCharacter> Characters { get { return new ReadOnlyObservableCollection<MediaCharacter>(_characters); } }

        public MediaMetadata()
        {
            _tags = new ObservableCollection<string>();
            _characters = new ObservableCollection<MediaCharacter>();
        }
    }
}
