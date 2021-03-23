using MvvmCross;
using MvvmCross.Core;
using MvvmCross.Platforms.Wpf.Core;
using MvvmCross.Platforms.Wpf.Views;
using MediaExplorer.Core.Services;
using MediaExplorer.Wpf.Services;

namespace MediaExplorer.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : MvxApplication
    {
        public App()
        {
            this.RegisterSetupType<MvxWpfSetup<Core.App>>();
        }
    }
}
