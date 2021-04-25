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

        public MvxObservableCollection<TagViewModel> Tags { get; private set; }

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

        private IMvxCommand _addTagCommand;
        public IMvxCommand AddTagCommand =>
            _addTagCommand ?? (_addTagCommand = new MvxCommand(AddTag, AddTagCanExecute));

        public MediaMetadataViewModel(MediaMetadata metadata)
        {
            _metadata = metadata;
            ((INotifyCollectionChanged)_metadata.Tags).CollectionChanged += TagsChanged;
            Tags = new MvxObservableCollection<TagViewModel>();
            foreach(MediaTag tag in _metadata.Tags)
            {
                Tags.Add(new TagViewModel(tag, TagDeleted));
            }
            NewTag = string.Empty;
        }

        private void TagsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach(MediaTag tag in e.NewItems)
                {
                    Tags.Add(new TagViewModel(tag, TagDeleted));
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

        private void TagDeleted(object sender, EventArgs e)
        {
            _metadata.RemoveTag((sender as TagViewModel).Text);
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
    }
}
