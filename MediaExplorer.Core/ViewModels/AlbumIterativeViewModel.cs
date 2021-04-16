using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Core.ViewModels
{
    public class AlbumIterativeViewModel : GeneralAlbumViewModel
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
                    var vm = new MediaCollectionViewModel();
                    vm.Prepare(new Tuple<MediaCollection, string, byte[]>(Album.MediaCollections[It], Album.Name, Album.Key));
                    return vm;
                }
            }
        }

        public AlbumIterativeViewModel()
        {
            It = 0;
        }
    }
}
