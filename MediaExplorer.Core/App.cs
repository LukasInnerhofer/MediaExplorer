using MvvmCross.ViewModels;
using MvvmCross.IoC;
using MvvmCross;
using System.Security.Cryptography;
using System.IO;

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

            File.WriteAllBytes(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "placeholder.bmp", Resources.Placeholder);

            RegisterCustomAppStart<AppStart>();
        }
    }
}
