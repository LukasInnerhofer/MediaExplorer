using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace MediaExplorer.Core.ViewModels
{
    public class ProfileViewModel : MvxViewModel<Profile>
    { 
        private Profile _profile;

        private VirtualFolder _rootFolder;
        private VirtualFolder RootFolder
        {
            get { return _rootFolder; }
            set 
            {
                SetProperty(ref _rootFolder, value);

                ViewModels.Clear();
                foreach(VirtualFileSystemObject child in RootFolder.Children)
                {
                    if(child is VirtualFolder)
                    {
                        var viewModel = new VirtualFolderViewModel();
                        viewModel.Prepare(child as VirtualFolder);
                        ViewModels.Add(viewModel);
                    }
                    else
                    {
                        var viewModel = new VirtualAlbumFileViewModel();
                        viewModel.Prepare(child as VirtualAlbumFile);
                        ViewModels.Add(viewModel);
                    }
                }
            }
        }

        private MvxObservableCollection<IVirtualFileSystemObjectViewModel> _viewModels;
        public MvxObservableCollection<IVirtualFileSystemObjectViewModel> ViewModels
        {
            get { return _viewModels; }
            set { SetProperty(ref _viewModels, value); }
        }

        private IVirtualFileSystemObjectViewModel _selectedViewModel;
        public IVirtualFileSystemObjectViewModel SelectedViewModel 
        {
            get { return _selectedViewModel; }
            set { SetProperty(ref _selectedViewModel, value); }
        }

        private IMvxCommand _newFolderCommand;
        public IMvxCommand NewFolderCommand =>
            _newFolderCommand ?? (_newFolderCommand = new MvxCommand(NewFolder));

        private IMvxCommand _newAlbumCommand;
        public IMvxCommand NewAlbumCommand =>
            _newAlbumCommand ?? (_newAlbumCommand = new MvxCommand(NewAlbum));

        private IMvxCommand _startRenameCommand;
        public IMvxCommand StartRenameCommand =>
            _startRenameCommand ?? (_startRenameCommand = new MvxCommand(StartRename, StartRenameCanExecute));

        public ProfileViewModel()
        {
            _viewModels = new MvxObservableCollection<IVirtualFileSystemObjectViewModel>();
        }

        public override void Prepare(Profile parameter)
        {
            _profile = parameter;
            RootFolder = _profile.RootFolder;
        }

        private void NewFolder()
        {
            int counter = 1;
            string name = "New Folder";
            VirtualFolder folder;
            do
            {
                folder = new VirtualFolder(name, RootFolder);
                name = $"New Folder {counter++}";
            } while (!RootFolder.AddChild(folder));

            var viewModel = new VirtualFolderViewModel();
            viewModel.Prepare(folder);
            viewModel.IsNameReadOnly = false;
            ViewModels.Add(viewModel);
        }

        private void NewAlbum()
        {
            IOpenFolderDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetOpenFileDialog();
            if((bool)dialog.ShowDialog())
            {
                var album = Album.FromBasePath(dialog.SelectedPath);
                var albumFile = new VirtualAlbumFile(album, RootFolder);
                var viewModel = new VirtualAlbumFileViewModel();
                viewModel.Prepare(albumFile);
                viewModel.IsNameReadOnly = false;
                ViewModels.Add(viewModel);
            }
        }

        private void StartRename()
        {
            SelectedViewModel.StartRenameCommand?.Execute();
        }

        private bool StartRenameCanExecute()
        {
            return SelectedViewModel != null && SelectedViewModel.StartRenameCommand.CanExecute();
        }
    }
}
