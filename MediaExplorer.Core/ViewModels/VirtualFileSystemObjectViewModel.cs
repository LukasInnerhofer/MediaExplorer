using MvvmCross.ViewModels;
using MvvmCross.Commands;

namespace MediaExplorer.Core.ViewModels
{
    public abstract class VirtualFileSystemObjectViewModel : MvxViewModel
    {
        private bool _isNameReadOnly;
        public bool IsNameReadOnly
        {
            get { return _isNameReadOnly; }
            set { SetProperty(ref _isNameReadOnly, value); }
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
