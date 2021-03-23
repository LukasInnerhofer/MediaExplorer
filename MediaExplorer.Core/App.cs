using MvvmCross.ViewModels;
using MvvmCross.IoC;
using MvvmCross;

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
            // The previous line does not register Services with generic type parameters
            Mvx.IoCProvider.RegisterType(typeof(Services.ICryptographyService<>), typeof(Services.CryptographyService<>));

            RegisterCustomAppStart<AppStart>();
        }
    }
}
