using MediaExplorer.Core.Models;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.ViewModels
{
    public abstract class GeneralAlbumViewModel : MvxViewModel<Album>
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

        public GeneralAlbumViewModel()
        {

        }

        public override void Prepare(Album parameter)
        {
            Album = parameter;
        }
    }
}
