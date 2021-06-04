using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
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

        public MediaTagConditionViewModel TagFilter { get; private set; }
        public MediaCharacterConditionViewModel CharacterFilter { get; private set; }

        private IMvxCommand _saveCommand;
        public IMvxCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new MvxAsyncCommand(SaveAlbumAsync));

        private IMvxCommand _addMediaFromHttpCommand;
        public IMvxCommand AddMediaFromHttpCommand =>
            _addMediaFromHttpCommand ?? (_addMediaFromHttpCommand = new MvxAsyncCommand(AddMediaFromHttpAsync));

        private IMvxCommand _addMediaCollectionFromHttpCommand;
        public IMvxCommand AddMediaCollectionFromHttpCommand =>
            _addMediaCollectionFromHttpCommand ?? (_addMediaCollectionFromHttpCommand = new MvxAsyncCommand(AddMediaCollectionFromHttpAsync));

        private IMvxCommand _addMediaFromFileSystemCommand;
        public IMvxCommand AddMediaFromFileSystemCommand =>
            _addMediaFromFileSystemCommand ?? (_addMediaFromFileSystemCommand = new MvxAsyncCommand(AddMediaFromFileSystem));

        private IMvxCommand _closeCommand;
        public IMvxCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new MvxAsyncCommand(CloseAsync));

        public CommonAlbumViewModel()
        {
            TagFilter = new MediaTagConditionViewModel(new Condition(() => new MediaTag(string.Empty)));
            CharacterFilter = new MediaCharacterConditionViewModel(new Condition(() => new MediaCharacter(string.Empty)));
        }

        private async Task SaveAlbumAsync()
        {
            await Album.SaveAsync();
        }

        private async Task AddMediaFromHttpAsync()
        {
            await Album.AddMedia(
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().
                Navigate<HttpSourceDialogViewModel, object, List<Tuple<string, MemoryStream>>>(null));
        }

        private async Task AddMediaCollectionFromHttpAsync()
        {
            await Album.AddMediaCollection(
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().
                Navigate<HttpSourceDialogViewModel, object, List<Tuple<string, MemoryStream>>>(null));
        }

        private async Task AddMediaFromFileSystem()
        {
            IOpenFileDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetOpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Multiselect = true;
            if(dialog.ShowDialog() == OpenFileDialogResult.Ok)
            {
                await Album.AddMedia(new List<string>(dialog.FileNames));
            }
        }

        protected virtual async Task CloseAsync()
        {

        }
    }
}
