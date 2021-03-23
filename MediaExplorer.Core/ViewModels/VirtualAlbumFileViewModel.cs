using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Core.ViewModels
{
    public class VirtualAlbumFileViewModel : MvxViewModel<VirtualAlbumFile>, IVirtualFileSystemObjectViewModel
    {
        private VirtualAlbumFile _albumFile;
        private VirtualAlbumFile AlbumFile
        {
            get { return _albumFile; }
            set 
            {
                _albumFile = value;
                Name = _albumFile.Name;
            }
        }

        private string _name;
        public string Name 
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private bool _isNameReadOnly;
        public bool IsNameReadOnly
        {
            get { return _isNameReadOnly; }
            set { SetProperty(ref _isNameReadOnly, value); }
        }

        private IMvxCommand _startRenameCommand;
        public IMvxCommand StartRenameCommand =>
            _startRenameCommand ?? (_startRenameCommand = new MvxCommand(StartRename, StartRenameCanExecute));

        public VirtualAlbumFileViewModel() : base()
        {
            IsNameReadOnly = true;
        }

        public override void Prepare(VirtualAlbumFile parameter)
        {
            AlbumFile = parameter;
        }

        private void StartRename()
        {
            IsNameReadOnly = false;
        }

        private bool StartRenameCanExecute()
        {
            return IsNameReadOnly;
        }
    }
}
