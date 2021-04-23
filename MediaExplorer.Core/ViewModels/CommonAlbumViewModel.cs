using MediaExplorer.Core.Models;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.ViewModels
{
    public abstract class CommonAlbumViewModel : MvxViewModel<Album>
    {
        protected Album _album;
        protected Album Album
        {
            get { return _album; }
            set
            {
                _album = value;
                RaisePropertyChanged(nameof(MediaCollection));
            }
        }

        private IMvxCommand _addMediaFromHttpCommand;
        public IMvxCommand AddMediaFromHttpCommand =>
            _addMediaFromHttpCommand ?? (_addMediaFromHttpCommand = new MvxAsyncCommand(AddMediaFromHttpAsync));

        public CommonAlbumViewModel()
        {

        }

        private async Task AddMediaFromHttpAsync()
        {
            await Album.AddMedia(
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().
                Navigate<HttpSourceDialogViewModel, object, List<KeyValuePair<string, MemoryStream>>>(null));
        }
    }
}
