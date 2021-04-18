using MvvmCross.ViewModels;
using MvvmCross.IoC;
using MvvmCross;
using System.Security.Cryptography;

namespace MediaExplorer.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes().
                EndingWith("Service").
                AsInterfaces().
                RegisterAsLazySingleton();
            Mvx.IoCProvider.RegisterSingleton<Services.ICryptographyService>(new Services.CryptographyService(Aes.Create(), SHA256.Create()));

            RegisterCustomAppStart<AppStart>();
        }
    }
}
