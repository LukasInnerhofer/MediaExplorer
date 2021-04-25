using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;
using MvvmCross;
using MediaExplorer.Core.Services;
using System.Net;
using System.Threading;
using System.IO;
using MvvmCross.Navigation;

namespace MediaExplorer.Core.ViewModels
{
    public class VirtualAlbumFileViewModel : VirtualFileSystemObjectViewModel
    {
        private VirtualAlbumFile _albumFile;
        public  VirtualAlbumFile AlbumFile
        {
            get { return _albumFile; }
            private set 
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

        private IMvxCommand _confirmRenameCommand;
        public IMvxCommand ConfirmRenameCommand =>
            _confirmRenameCommand ?? (_confirmRenameCommand = new MvxCommand(ConfirmRename));

        private IMvxCommand _cancelRenameCommand;
        public IMvxCommand CancelRenameCommand =>
            _cancelRenameCommand ?? (_cancelRenameCommand = new MvxCommand(CancelRename));

        private IMvxCommand _openCommand;
        public IMvxCommand OpenCommand =>
            _openCommand ?? (_openCommand = new MvxCommand(Open));

        public VirtualAlbumFileViewModel(VirtualAlbumFile albumFile) : base()
        {
            AlbumFile = albumFile;
            IsNameReadOnly = true;
        }

        private void ConfirmRename()
        {
            try
            {
                AlbumFile.Rename(Name);
                IsNameReadOnly = true;
            }
            catch (Exception e)
            {
                string errorMessage = "Unknown error.";
                if (e is ArgumentException)
                {
                    errorMessage = "New name must not be empty.";
                }
                else if (e is VirtualFileSystemObjectExistsException)
                {
                    errorMessage = $"An album with the name \"{Name}\" already exists.";
                }
                Name = AlbumFile.Name;
                Mvx.IoCProvider.Resolve<IMessageBoxService>().ShowMessageBox(errorMessage);
            }
        }

        private void CancelRename()
        {
            Name = AlbumFile.Name;
            IsNameReadOnly = true;
        }

        private void Open()
        {
            if(IsNameReadOnly)
            {
                Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(new IterativeAlbumViewModel(), _albumFile.Album);
            }
        }
    }
}
