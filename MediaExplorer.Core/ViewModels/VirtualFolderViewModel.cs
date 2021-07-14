using MvvmCross.ViewModels;
using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross.Commands;
using System;
using MvvmCross;

namespace MediaExplorer.Core.ViewModels
{
    public class VirtualFolderViewModel : VirtualFileSystemObjectViewModel
    {
        private VirtualFolder _folder;
        public VirtualFolder Folder
        {
            get { return _folder; }
            private set
            {
                _folder = value;
                Name = _folder.Name;
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

        private VirtualFolderViewModel(VirtualFolder folder, bool isNameReadOnly) : base()
        {
            Folder = folder;
            IsNameReadOnly = isNameReadOnly;
        }

        public VirtualFolderViewModel() : this(null)
        {

        }

        public VirtualFolderViewModel(VirtualFolder folder) : this(folder, true)
        {

        }

        private void ConfirmRename()
        {
            try
            {
                Folder.Rename(Name);
                IsNameReadOnly = true;
            }
            catch(Exception e)
            {
                string errorMessage = "Unknown error.";
                if(e is ArgumentException)
                {
                    errorMessage = "New name must not be empty.";
                }
                else if(e is VirtualFileSystemObjectExistsException)
                {
                    errorMessage = $"A folder with the name \"{Name}\" already exists.";
                }
                Name = Folder.Name;
                Mvx.IoCProvider.Resolve<IMessageBoxService>().ShowAsync(
                    errorMessage, 
                    "Error Renaming Folder", 
                    MessageBoxButton.Ok, 
                    MessageBoxImage.Error, 
                    MessageBoxResult.Ok).Wait();
            }
        }

        private void CancelRename()
        {
            Name = Folder.Name;
            IsNameReadOnly = true;
        }
    }
}
