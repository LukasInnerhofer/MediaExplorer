using System.Windows;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace MediaExplorer.Wpf.Views
{
    /// <summary>
    /// Interaction logic for OpenProfileView.xaml
    /// </summary>
    [MvxViewFor(typeof(Core.ViewModels.OpenProfileViewModel))]
    public partial class OpenProfileView : MvxWpfView
    {
        public OpenProfileView()
        {
            InitializeComponent();
        }

        private void PwdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext != null)
            {
                (DataContext as Core.ViewModels.OpenProfileViewModel).Key =
                    (sender as PasswordBox).Password;
            }
        }
    }
}
