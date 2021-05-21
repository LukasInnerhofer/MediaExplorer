using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using MediaExplorer.Core.ViewModels;
using MvvmCross;
using MediaExplorer.Core.Services;
using MediaExplorer.Uwp.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaExplorer.Uwp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    [MvxViewFor(typeof(OpenProfileViewModel))]
    public sealed partial class OpenProfileView : MvxWindowsPage
    {
        public OpenProfileView()
        {
            this.InitializeComponent();
            Mvx.IoCProvider.RegisterSingleton<IFileSystemService>(new FileSystemService());
        }
    }
}
