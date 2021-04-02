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
    public class OpenProfileViewModel : MvxViewModel
    {
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

        public override void Prepare()
        {
            base.Prepare();
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            if (!File.Exists(Constants.File.Profile))
            {
                Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<CreateProfileViewModel>();
            }
        }

        private async Task Open()
        {
            Models.Profile profile;
            using(var fs = new FileStream("profile", FileMode.Open))
            {
                byte[] keyHash = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
                profile = await Mvx.IoCProvider.Resolve<ICryptographyService>().DeserializeAsync<Models.Profile>(fs, keyHash);
                profile.KeyHash = keyHash;
            }
            
            await Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate(new ProfileViewModel(), profile);
        }

        private bool OpenCanExecute()
        {
            return Key != string.Empty;
        }
    }
}
