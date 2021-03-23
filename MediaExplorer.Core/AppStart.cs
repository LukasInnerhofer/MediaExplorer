using MvvmCross.ViewModels;
using MvvmCross.Navigation;
using System.Threading.Tasks;

namespace MediaExplorer.Core
{
    class AppStart : MvxAppStart
    {
        public AppStart(IMvxApplication app, IMvxNavigationService navigationService) : base(app, navigationService)
        {

        }

        protected override Task NavigateToFirstViewModel(object hint = null)
        {
            return NavigationService.Navigate<ViewModels.OpenProfileViewModel>();
        }
    }
}
