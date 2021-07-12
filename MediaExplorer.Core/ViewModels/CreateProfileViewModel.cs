using MediaExplorer.Core.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.ViewModels
{
    public class CreateProfileViewModel : MvxViewModel<string>
    {
        private string _profilePath;

        private string _key;
        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                CreateCommand.RaiseCanExecuteChanged();
            }
        }

        private IMvxCommand _createCommand;
        public IMvxCommand CreateCommand => 
            _createCommand ?? (_createCommand = new MvxAsyncCommand(Create, CreateCanExecute));

        public CreateProfileViewModel()
        {

        }

        public override void Prepare(string profilePath)
        {
            Key = string.Empty;
            _profilePath = profilePath;
        }

        private async Task Create()
        {
            await Mvx.IoCProvider.Resolve<IFileSystemService>().CreateFileAsync(_profilePath);
            using (var fs = await Mvx.IoCProvider.Resolve<IFileSystemService>().OpenFileAsync(_profilePath))
            {
                await Mvx.IoCProvider.Resolve<ICryptographyService>().SerializeAsync(
                    fs,
                    new Models.Profile(),
                    SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Key)));
            }
            
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Close(this);
        }

        private bool CreateCanExecute()
        {
            return Key != string.Empty;
        }
    }
}
