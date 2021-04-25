using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    public class MediaTagViewModel : MvxViewModel
    {
        public string Text
        {
            get { return Tag.Text; }
        }

        private MediaTag _tag;
        public MediaTag Tag 
        { 
            get { return _tag; }
            private set
            {
                SetProperty(ref _tag, value);
                RaisePropertyChanged(nameof(Text));
            }
        }

        private IMvxCommand _deleteCommand;
        public IMvxCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new MvxCommand(Delete));

        public event EventHandler Deleted;

        public MediaTagViewModel(MediaTag tag, EventHandler deleted = null)
        {
            Tag = tag;
            if(deleted != null)
            {
                Deleted += deleted;
            }
        }

        private void Delete()
        {
            Deleted?.Invoke(this, new EventArgs());
        }
    }
}
