using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using System.Collections.Specialized;

namespace MediaExplorer.Core.ViewModels
{
    public class IterativeAlbumViewModel : CommonAlbumViewModel
    {
        private int _it;
        private int It
        {
            get { return _it; }
            set
            {
                _it = value;
                RaisePropertyChanged(nameof(MediaCollection));
                NavigateNextCommand.RaiseCanExecuteChanged();
                NavigatePreviousCommand.RaiseCanExecuteChanged();
            }
        }

        public MediaCollectionViewModel MediaCollection
        {
            get
            {
                if(Album.MediaCollections.Count <= It)
                {
                    return null;
                }
                else
                {
                    return new MediaCollectionViewModel(Album.MediaCollections[It], Album.Name, Album.Key);
                }
            }
        }

        private IMvxCommand _navigateNextCommand;
        public IMvxCommand NavigateNextCommand =>
            _navigateNextCommand ?? (_navigateNextCommand = new MvxCommand(NavigateNext, NavigateNextCanExecute));

        private IMvxCommand _navigatePreviousCommand;
        public IMvxCommand NavigatePreviousCommand =>
            _navigatePreviousCommand ?? (_navigatePreviousCommand = new MvxCommand(NavigatePrevious, NavigatePreviousCanExecute));

        public IterativeAlbumViewModel()
        {
            It = 0;
        }

        public override void Prepare(Album parameter)
        {
            Album = parameter;
            ((INotifyCollectionChanged)Album.MediaCollections).CollectionChanged += MediaCollectionsChanged;
        }

        private void MediaCollectionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NavigateNextCommand.RaiseCanExecuteChanged();
            NavigatePreviousCommand.RaiseCanExecuteChanged();
        }

        private void NavigateNext()
        {
            ++It;
        }

        private bool NavigateNextCanExecute()
        {
            return It < Album.MediaCollections.Count - 1;
        }

        private void NavigatePrevious()
        {
            --It;
        }

        private bool NavigatePreviousCanExecute()
        {
            return It > 0;
        }
    }
}
