using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using System.IO;
using MediaExplorer.Core.Services;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MvvmCross;

namespace MediaExplorer.Core.ViewModels
{
    public class OpenProfileViewModel : MvxViewModel<string>
    {
        private string _profilePath;

        private string _key;
        public string Key
        {
            private get { return _key; }
            set
            {
                _key = value;
                OpenCommand.RaiseCanExecuteChanged();
            }
        }

        private IMvxCommand _openCommand;
        public IMvxCommand OpenCommand =>
            _openCommand ?? (_openCommand = new MvxAsyncCommand(Open, OpenCanExecute));

        public OpenProfileViewModel()
        {
            Key = string.Empty;
        }

        public override void Prepare(string profilePath)
        {
            base.Prepare();

            _profilePath = profilePath;
        }

        private async Task Open()
        {
            Models.Profile profile;
            using(var fs = await Mvx.IoCProvider.Resolve<IFileSystemService>().OpenFileAsync(_profilePath))
            {
                byte[] keyHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));

                try
                {
                    profile = await Mvx.IoCProvider.Resolve<ICryptographyService>().DeserializeAsync<Models.Profile>(fs, keyHash);
                }
                catch(InvalidKeyException)
                {
                    await Mvx.IoCProvider.Resolve<IMessageBoxService>().ShowAsync(
                        "Invalid key.", 
                        "Error Opening Profile", 
                        MessageBoxButton.Ok, 
                        MessageBoxImage.Error, 
                        MessageBoxResult.Ok);
                    return;
                }
                await profile.InitializeNonSerializedMembers(keyHash, _profilePath);
            }
            
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(new ProfileViewModel(), profile);
        }

        private bool OpenCanExecute()
        {
            return Key != string.Empty;
        }
    }
}
