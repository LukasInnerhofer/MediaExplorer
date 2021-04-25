using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    public class MediaCharacterViewModel : MvxViewModel
    {
        private MediaCharacter _character;
        public MediaCharacter Character
        {
            get { return _character; }
            private set
            {
                _character = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public event EventHandler Deleted;

        public string Name { get { return _character.Name; } }

        private IMvxCommand _deleteCommand;
        public IMvxCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new MvxCommand(Delete));

        private IMvxCommand _addTagCommand;
        public IMvxCommand AddTagCommand =>
            _addTagCommand ?? (_addTagCommand = new MvxCommand(AddTag, AddTagCanExecute));

        public MvxObservableCollection<MediaTagViewModel> Tags { get; private set; }

        private string _newTag;
        public string NewTag
        {
            get { return _newTag; }
            set
            {
                SetProperty(ref _newTag, value);
                AddTagCommand.RaiseCanExecuteChanged();
            }
        }

        public MediaCharacterViewModel(MediaCharacter character, EventHandler deleted = null)
        {
            Character = character;

            ((INotifyCollectionChanged)Character.Tags).CollectionChanged += TagsChanged;
            Tags = new MvxObservableCollection<MediaTagViewModel>();
            foreach(MediaTag tag in character.Tags)
            {
                Tags.Add(new MediaTagViewModel(tag, TagDeleted));
            }

            NewTag = string.Empty;

            if(deleted != null)
            {
                Deleted += deleted;
            }
        }

        private void TagsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MediaTag tag in e.NewItems)
                {
                    Tags.Add(new MediaTagViewModel(tag, TagDeleted));
                }
            }
            if (e.OldItems != null)
            {
                foreach (MediaTag tag in e.OldItems)
                {
                    Tags.Remove(Tags.First(x => x.Tag == tag));
                }
            }
        }

        private void TagDeleted(object sender, EventArgs e)
        {
            _character.RemoveTag((sender as MediaTagViewModel).Text);
        }

        private void AddTag()
        {
            _character.AddTag(NewTag);
            NewTag = string.Empty;
        }

        private bool AddTagCanExecute()
        {
            return NewTag != string.Empty;
        }

        private void Delete()
        {
            Deleted?.Invoke(this, new EventArgs());
        }
    }
}
