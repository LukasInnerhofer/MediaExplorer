using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    public class MediaMetadataViewModel : MvxViewModel
    {
        private MediaMetadata _metadata;

        public MvxObservableCollection<MediaTagViewModel> Tags { get; private set; }
        public MvxObservableCollection<MediaCharacterViewModel> Characters { get; private set; }

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

        private string _newCharacterName;
        public string NewCharacterName
        {
            get { return _newCharacterName; }
            set
            {
                SetProperty(ref _newCharacterName, value);
                AddCharacterCommand.RaiseCanExecuteChanged();
            }
        }

        private IMvxCommand _addTagCommand;
        public IMvxCommand AddTagCommand =>
            _addTagCommand ?? (_addTagCommand = new MvxCommand(AddTag, AddTagCanExecute));

        private IMvxCommand _addCharacterCommand;
        public IMvxCommand AddCharacterCommand =>
            _addCharacterCommand ?? (_addCharacterCommand = new MvxCommand(AddCharacter, AddCharacterCanExecute));

        public MediaMetadataViewModel(MediaMetadata metadata)
        {
            _metadata = metadata;

            ((INotifyCollectionChanged)_metadata.Tags).CollectionChanged += TagsChanged;
            Tags = new MvxObservableCollection<MediaTagViewModel>();
            foreach(MediaTag tag in _metadata.Tags)
            {
                Tags.Add(new MediaTagViewModel(tag, TagDeleted));
            }

            ((INotifyCollectionChanged)_metadata.Characters).CollectionChanged += CharactersChanged;
            Characters = new MvxObservableCollection<MediaCharacterViewModel>();
            foreach(MediaCharacter character in _metadata.Characters)
            {
                Characters.Add(new MediaCharacterViewModel(character, CharacterDeleted));
            }

            NewTag = string.Empty;
            NewCharacterName = string.Empty;
        }

        private void CharactersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (MediaCharacter character in e.NewItems)
                {
                    Characters.Add(new MediaCharacterViewModel(character, CharacterDeleted));
                }
            }
            if (e.OldItems != null)
            {
                foreach (MediaCharacter character in e.OldItems)
                {
                    Characters.Remove(Characters.First(x => x.Character == character));
                }
            }
        }

        private void TagsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(MediaTag tag in e.NewItems)
                {
                    Tags.Add(new MediaTagViewModel(tag, TagDeleted));
                }
            }
            if(e.OldItems != null)
            {
                foreach(MediaTag tag in e.OldItems)
                {
                    Tags.Remove(Tags.First(x => x.Tag == tag));
                }
            }
        }

        private void CharacterDeleted(object sender, NotifyCollectionChangedEventArgs e)
        {
            _metadata.RemoveCharacter((sender as MediaCharacterViewModel).Name);
        }

        private void TagDeleted(object sender, EventArgs e)
        {
            _metadata.RemoveTag((sender as MediaTagViewModel).Text);
        }

        private void CharacterDeleted(object sender, EventArgs e)
        {
            _metadata.RemoveCharacter((sender as MediaCharacterViewModel).Name);
        }

        private void AddTag()
        {
            _metadata.AddTag(NewTag);
            NewTag = string.Empty;
        }

        private bool AddTagCanExecute()
        {
            return NewTag != string.Empty;
        }

        private void AddCharacter()
        {
            _metadata.AddCharacter(NewCharacterName);
            NewCharacterName = string.Empty;
        }

        private bool AddCharacterCanExecute()
        {
            return NewCharacterName != string.Empty;
        }
    }
}
