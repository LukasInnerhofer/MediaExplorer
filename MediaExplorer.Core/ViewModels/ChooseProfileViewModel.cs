using System;
using System.Collections.Generic;
using System.Text;
using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using MvvmCross.Commands;
using MvvmCross;
using MediaExplorer.Core.Services;
using MediaExplorer.Core.Models;
using System.Threading.Tasks;

namespace MediaExplorer.Core.ViewModels
{
    public class ChooseProfileViewModel : MvxViewModel
    {
        private IMvxCommand _createProfileCommand;
        public IMvxCommand CreateProfileCommand =>
            _createProfileCommand ?? (_createProfileCommand = new MvxAsyncCommand(CreateProfile));

        private IMvxCommand _openProfileCommand;
        public IMvxCommand OpenProfileCommand =>
            _openProfileCommand ?? (_openProfileCommand = new MvxAsyncCommand(OpenProfile));

        private async Task CreateProfile()
        {
            ICreateFileDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetCreateFileDialog();
            dialog.RestoreDirectory = true;
            if (await dialog.ShowDialogAsync() == CreateFileDialogResult.Ok)
            {
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(new CreateProfileViewModel(), dialog.FileName);
            }
        }

        private async Task OpenProfile()
        {
            IOpenFileDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetOpenFileDialog();
            dialog.RestoreDirectory = true;
            dialog.Filter.Add("Profiles", new List<string>() { ".media_explorer_profile" });
            if (await dialog.ShowDialogAsync() == OpenFileDialogResult.Ok)
            {
                await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(new OpenProfileViewModel(), dialog.FileName);
            }
        }
    }
}
