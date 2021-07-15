using MvvmCross.ViewModels;
using MvvmCross.Commands;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Core.ViewModels
{
    public abstract class VirtualFileSystemObjectViewModel : MvxViewModel
    {
        protected VirtualFileSystemObject _virtualFileSystemObject;

        public bool IsNameReadOnly
        {
            get { return _virtualFileSystemObject.IsNameReadOnly; }
            set { _virtualFileSystemObject.IsNameReadOnly = value; RaisePropertyChanged(); }
        }

        private IMvxCommand _startRenameCommand;
        public IMvxCommand StartRenameCommand =>
            _startRenameCommand ?? (_startRenameCommand = new MvxCommand(StartRename, StartRenameCanExecute));

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
