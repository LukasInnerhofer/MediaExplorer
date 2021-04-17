using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;
using MvvmCross.Commands;

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

        public IterativeAlbumViewModel()
        {
            It = 0;
        }

        private void NavigateNext()
        {
            ++It;
            RaisePropertyChanged(nameof(MediaCollection));
        }

        private bool NavigateNextCanExecute()
        {
            return It < Album.MediaCollections.Count - 1;
        }
    }
}
