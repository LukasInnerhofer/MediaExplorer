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
        private int _itCollections;
        private int ItCollections
        {
            get { return _itCollections; }
            set
            {
                _itCollections = value;
                ItMedia = 0;
                NavigateNextCommand.RaiseCanExecuteChanged();
                NavigatePreviousCommand.RaiseCanExecuteChanged();
            }
        }

        private int _itMedia;
        private int ItMedia
        {
            get { return _itMedia; }
            set
            {
                _itMedia = value;
                RaisePropertyChanged(nameof(Media));
                NavigateNextMediaCommand.RaiseCanExecuteChanged();
                NavigatePreviousMediaCommand.RaiseCanExecuteChanged();
            }
        }

        public MediaViewModel Media
        {
            get
            {
                return new MediaViewModel(Album.MediaCollections[ItCollections].Media[ItMedia], Album.MediaCollections[ItCollections], Album.Name, Album.Key);
            }
        }

        private IMvxCommand _navigateNextCommand;
        public IMvxCommand NavigateNextCommand =>
            _navigateNextCommand ?? (_navigateNextCommand = new MvxCommand(NavigateNext, NavigateNextCanExecute));

        private IMvxCommand _navigatePreviousCommand;
        public IMvxCommand NavigatePreviousCommand =>
            _navigatePreviousCommand ?? (_navigatePreviousCommand = new MvxCommand(NavigatePrevious, NavigatePreviousCanExecute));

        private IMvxCommand _navigateNextMediaCommand;
        public IMvxCommand NavigateNextMediaCommand =>
            _navigateNextMediaCommand ?? (_navigateNextMediaCommand = new MvxCommand(NavigateNextMedia, NavigateNextMediaCanExecute));

        private IMvxCommand _navigatePreviousMediaCommand;
        public IMvxCommand NavigatePreviousMediaCommand =>
            _navigatePreviousMediaCommand ?? (_navigatePreviousMediaCommand = new MvxCommand(NavigatePreviousMedia, NavigatePreviousMediaCanExecute));

        public IterativeAlbumViewModel()
        {
            ItCollections = 0;
            ItMedia = 0;
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
            ++ItCollections;
        }

        private bool NavigateNextCanExecute()
        {
            return ItCollections < Album.MediaCollections.Count - 1;
        }

        private void NavigatePrevious()
        {
            --ItCollections;
        }

        private bool NavigatePreviousCanExecute()
        {
            return ItCollections > 0;
        }

        private void NavigateNextMedia()
        {
            ++ItMedia;
        }

        private bool NavigateNextMediaCanExecute()
        {
            return ItMedia < Album.MediaCollections[ItCollections].Media.Count - 1;
        }

        private void NavigatePreviousMedia()
        {
            --ItMedia;
        }

        private bool NavigatePreviousMediaCanExecute()
        {
            return ItMedia > 0;
        }
    }
}
