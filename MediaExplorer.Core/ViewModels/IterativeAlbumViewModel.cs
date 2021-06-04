using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using System.Collections.Specialized;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Navigation;
using System.Linq;

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
                NavigateBeginCommand.RaiseCanExecuteChanged();
                NavigateEndCommand.RaiseCanExecuteChanged();
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
                RaisePropertyChanged(nameof(MediaMetadata));
                NavigateNextMediaCommand.RaiseCanExecuteChanged();
                NavigatePreviousMediaCommand.RaiseCanExecuteChanged();
            }
        }

        private int _newIt;

        private MediaViewModel _media;
        public MediaViewModel Media
        {
            get
            {
                if(Album.MediaCollections.Count > 0)
                {
                    _media?.Close();
                    _media = new MediaViewModel(Album.MediaCollections[ItCollections].Media[ItMedia], Album.MediaCollections[ItCollections], Album.Name, Album.Key);
                    return _media;
                }
                else
                {
                    return null;
                }
            }
        }

        public MediaMetadataViewModel MediaMetadata => Media.Metadata;

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

        private IMvxCommand _navigateBeginCommand;
        public IMvxCommand NavigateBeginCommand =>
            _navigateBeginCommand ?? (_navigateBeginCommand = new MvxCommand(NavigateBegin, NavigateBeginCanExecute));

        private IMvxCommand _navigateEndCommand;
        public IMvxCommand NavigateEndCommand =>
            _navigateEndCommand ?? (_navigateEndCommand = new MvxCommand(NavigateEnd, NavigateEndCanExecute));

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
            RaisePropertyChanged(nameof(Media));
            RaisePropertyChanged(nameof(MediaMetadata));
            NavigateNextCommand.RaiseCanExecuteChanged();
            NavigatePreviousCommand.RaiseCanExecuteChanged();
            NavigateBeginCommand.RaiseCanExecuteChanged();
            NavigateEndCommand.RaiseCanExecuteChanged();
        }

        private bool FilterTag(object tag)
        {
            return Album.MediaCollections[_newIt].Media.First().Metadata.Tags.Any(x => x.Text == ((MediaTag)tag).Text) || ((MediaTag)tag).Text == string.Empty;
        }

        private bool FilterCharacter(object character)
        {
            return Album.MediaCollections[_newIt].Media.First().Metadata.Characters.Any(x => x.Name == ((MediaCharacter)character).Name) || ((MediaCharacter)character).Name == string.Empty;
        }

        private void NavigateNext()
        {
            _newIt = ItCollections;
            do
            {
                ++_newIt;
                if (_newIt >= Album.MediaCollections.Count) return;
            } while (!TagFilter.Cond.Evaluate(FilterTag) || !CharacterFilter.Cond.Evaluate(FilterCharacter));
            ItCollections = _newIt;
        }

        private bool NavigateNextCanExecute()
        {
            return ItCollections < Album.MediaCollections.Count - 1;
        }

        private void NavigatePrevious()
        {
            _newIt = ItCollections;
            do
            {
                --_newIt;
                if (_newIt == -1) return;
            } while (!TagFilter.Cond.Evaluate(FilterTag) || !CharacterFilter.Cond.Evaluate(FilterCharacter));
            ItCollections = _newIt;
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
            return Album.MediaCollections.Count > 0 && ItMedia < Album.MediaCollections[ItCollections].Media.Count - 1;
        }

        private void NavigatePreviousMedia()
        {
            --ItMedia;
        }

        private bool NavigatePreviousMediaCanExecute()
        {
            return ItMedia > 0;
        }

        private void NavigateBegin()
        {
            ItMedia = 0;
            ItCollections = 0;
        }

        private bool NavigateBeginCanExecute()
        {
            return ItCollections > 0;
        }

        private void NavigateEnd()
        {
            ItMedia = 0;
            ItCollections = Album.MediaCollections.Count - 1;
        }

        private bool NavigateEndCanExecute()
        {
            return ItCollections < Album.MediaCollections.Count - 1 && Album.MediaCollections.Count > 0;
        }

        protected override async Task CloseAsync()
        {
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this);
        }
    }
}
