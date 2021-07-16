using MediaExplorer.Core.Services;
using MediaExplorer.Wpf.Services;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;

namespace MediaExplorer.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Mvx.IoCProvider.RegisterSingleton<IMessageBoxService>(new MessageBoxService());
            Mvx.IoCProvider.RegisterSingleton<IFileDialogService>(new FileDialogService());
            Mvx.IoCProvider.RegisterSingleton<IFileSystemService>(new FileSystemService());
        }
    }
}
