using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                Name = _character.Name;
            }
        }

        public event EventHandler Deleted;

        private string _name;
        public string Name 
        { 
            get { return _name; } 
            set { SetProperty(ref _name, value); }
        }

        private IMvxCommand _deleteCommand;
        public IMvxCommand DeleteCommand =>
            _deleteCommand ?? (_deleteCommand = new MvxCommand(Delete));

        private IMvxCommand _addTagCommand;
        public IMvxCommand AddTagCommand =>
            _addTagCommand ?? (_addTagCommand = new MvxCommand(AddTag, AddTagCanExecute));

        private IMvxCommand _startRenameCommand;
        public IMvxCommand StartRenameCommand =>
            _startRenameCommand ?? (_startRenameCommand = new MvxCommand(StartRename, StartRenameCanExecute));

        private IMvxCommand _confirmRenameCommand;
        public IMvxCommand ConfirmRenameCommand =>
            _confirmRenameCommand ?? (_confirmRenameCommand = new MvxCommand(ConfirmRename));

        private IMvxCommand _cancelRenameCommand;
        public IMvxCommand CancelRenameCommand =>
            _cancelRenameCommand ?? (_cancelRenameCommand = new MvxCommand(CancelRename));

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

        private bool _isNameReadOnly;
        public bool IsNameReadOnly
        {
            get { return _isNameReadOnly; }
            set
            {
                SetProperty(ref _isNameReadOnly, value);
                StartRenameCommand.RaiseCanExecuteChanged();
            }
        }

        public ReadOnlyObservableCollection<string> AllCharacterTags { get; private set; }

        private MediaCharacterViewModel(MediaCharacter character, bool isNameReadOnly, EventHandler deleted, ReadOnlyObservableCollection<string> allCharacterTags)
        {
            Character = character;

            ((INotifyCollectionChanged)Character.Tags).CollectionChanged += TagsChanged;
            Tags = new MvxObservableCollection<MediaTagViewModel>();
            foreach (MediaTag tag in character.Tags)
            {
                Tags.Add(new MediaTagViewModel(tag, TagDeleted));
            }

            NewTag = string.Empty;

            IsNameReadOnly = isNameReadOnly;

            if (deleted != null)
            {
                Deleted += deleted;
            }

            AllCharacterTags = allCharacterTags;
        }

        public MediaCharacterViewModel(MediaCharacter character, ReadOnlyObservableCollection<string> allCharacterTags) : this(character, true, null, allCharacterTags)
        {

        }

        public MediaCharacterViewModel(MediaCharacter character, EventHandler deleted, ReadOnlyObservableCollection<string> allCharacterTags) : this(character, true, deleted, allCharacterTags)
        {
            
        }

        public MediaCharacterViewModel(MediaCharacter character, bool isNameReadOnly, ReadOnlyObservableCollection<string> allCharacterTags) : this(character, isNameReadOnly, null, allCharacterTags)
        {

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

        private void StartRename()
        {
            IsNameReadOnly = false;
        }

        private bool StartRenameCanExecute()
        {
            return IsNameReadOnly;
        }

        private void ConfirmRename()
        {
            try
            {
                Character.Rename(Name);
                IsNameReadOnly = true;
            }
            catch(ArgumentException)
            {
                Mvx.IoCProvider.Resolve<IMessageBoxService>().ShowAsync(
                    "New name must not be empty", 
                    "Error Renaming Character", 
                    MessageBoxButton.Ok, 
                    MessageBoxImage.Error, 
                    MessageBoxResult.Ok).Wait();
                Name = Character.Name;
            }
        }

        private void CancelRename()
        {
            Name = Character.Name;
            IsNameReadOnly = true;
        }
    }
}
