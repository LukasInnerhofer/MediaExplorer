using MvvmCross.ViewModels;
using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross.Commands;
using System;
using MvvmCross;

namespace MediaExplorer.Core.ViewModels
{
    public class VirtualFolderViewModel : MvxViewModel<VirtualFolder>, IVirtualFileSystemObjectViewModel
    {
        private VirtualFolder _folder;
        private VirtualFolder Folder
        {
            get { return _folder; }
            set
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
        private bool _isNameReadOnly;
        public bool IsNameReadOnly 
        { 
            get { return _isNameReadOnly; } 
            set { SetProperty(ref _isNameReadOnly, value); }
        }

        private IMvxCommand _startRenameCommand;
        public IMvxCommand StartRenameCommand =>
            _startRenameCommand ?? (_startRenameCommand = new MvxCommand(StartRename, StartRenameCanExecute));

        private IMvxCommand _confirmRenameCommand;
        public IMvxCommand ConfirmRenameCommand =>
            _confirmRenameCommand ?? (_confirmRenameCommand = new MvxCommand(ConfirmRename));

        private IMvxCommand _cancelRenameCommand;
        public IMvxCommand CancelRenameCommand =>
            _cancelRenameCommand ?? (_cancelRenameCommand = new MvxCommand(CancelRename));

        public VirtualFolderViewModel() : base()
        {
            IsNameReadOnly = true;
        }

        public override void Prepare(VirtualFolder parameter)
        {
            Folder = parameter;
        }

        private void StartRename()
        {
            IsNameReadOnly = false;
        }

        private bool StartRenameCanExecute()
        {
            return IsNameReadOnly;
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
                Mvx.IoCProvider.Resolve<IMessageBoxService>().ShowMessageBox(errorMessage);
            }
        }

        private void CancelRename()
        {
            Name = Folder.Name;
            IsNameReadOnly = true;
        }
    }
}
