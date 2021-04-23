using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.IO;
using System.Threading.Tasks;

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
                        ViewModels.Add(new VirtualFolderViewModel(child as VirtualFolder));
                    }
                    else
                    {
                        ViewModels.Add(new VirtualAlbumFileViewModel(child as VirtualAlbumFile));
                    }
                }
            }
        }

        private MvxObservableCollection<VirtualFileSystemObjectViewModel> _viewModels;
        public MvxObservableCollection<VirtualFileSystemObjectViewModel> ViewModels
        {
            get { return _viewModels; }
            set { SetProperty(ref _viewModels, value); }
        }

        private VirtualFileSystemObjectViewModel _selectedViewModel;
        public VirtualFileSystemObjectViewModel SelectedViewModel 
        {
            get { return _selectedViewModel; }
            set { SetProperty(ref _selectedViewModel, value); }
        }

        private IMvxCommand _saveCommand;
        public IMvxCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new MvxAsyncCommand(Save));

        private IMvxCommand _newFolderCommand;
        public IMvxCommand NewFolderCommand =>
            _newFolderCommand ?? (_newFolderCommand = new MvxCommand(NewFolder));

        private IMvxCommand _newAlbumCommand;
        public IMvxCommand NewAlbumCommand =>
            _newAlbumCommand ?? (_newAlbumCommand = new MvxAsyncCommand(NewAlbumAsync));

        private IMvxCommand _startRenameCommand;
        public IMvxCommand StartRenameCommand =>
            _startRenameCommand ?? (_startRenameCommand = new MvxCommand(StartRename, StartRenameCanExecute));

        private IMvxCommand _openCommand;
        public IMvxCommand OpenCommand =>
            _openCommand ?? (_openCommand = new MvxCommand(Open));

        public ProfileViewModel()
        {
            _viewModels = new MvxObservableCollection<VirtualFileSystemObjectViewModel>();
        }

        public override void Prepare(Profile parameter)
        {
            _profile = parameter;
            RootFolder = _profile.RootFolder;
        }

        private async Task Save()
        {
            using(var fs = new FileStream(Constants.File.Profile, FileMode.Open))
            {
                await Mvx.IoCProvider.Resolve<ICryptographyService>().SerializeAsync(fs, _profile, _profile.KeyHash);
            }
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

            ViewModels.Add(new VirtualFolderViewModel(folder) { IsNameReadOnly = false });
        }

        private async Task NewAlbumAsync()
        {
            IOpenFolderDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetOpenFileDialog();
            if((bool)dialog.ShowDialog())
            {
                var album = await Album.FromBasePathAsync(dialog.SelectedPath, _profile.KeyHash);
                var albumFile = new VirtualAlbumFile(album, RootFolder);
                RootFolder.AddChild(albumFile);
                ViewModels.Add(new VirtualAlbumFileViewModel(albumFile) { IsNameReadOnly = false });
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

        private void Open()
        {
            if(SelectedViewModel is VirtualAlbumFileViewModel)
            {
                (SelectedViewModel as VirtualAlbumFileViewModel).OpenCommand?.Execute();
            }
            else
            {
                if(SelectedViewModel.IsNameReadOnly)
                {
                    RootFolder = (SelectedViewModel as VirtualFolderViewModel).Folder;
                }
            }
        }
    }
}
