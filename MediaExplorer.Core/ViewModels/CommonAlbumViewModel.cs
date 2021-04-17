using MediaExplorer.Core.Models;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
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

        private IMvxCommand _addMediaFromHtmlCommand;
        public IMvxCommand AddMediaFromHtmlCommand =>
            _addMediaFromHtmlCommand ?? (_addMediaFromHtmlCommand = new MvxAsyncCommand(AddMediaFromHtmlAsync));

        public CommonAlbumViewModel()
        {

        }

        public override void Prepare(Album parameter)
        {
            Album = parameter;
        }

        private async Task AddMediaFromHtmlAsync()
        {

        }
    }
}
