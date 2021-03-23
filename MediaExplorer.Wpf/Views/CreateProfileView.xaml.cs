using MvvmCross.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using System.Windows.Controls;

namespace MediaExplorer.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CreateProfileView.xaml
    /// </summary>
    [MvxViewFor(typeof(Core.ViewModels.CreateProfileViewModel))]
    public partial class CreateProfileView : MvxWpfView
    {
        public CreateProfileView()
        {
            InitializeComponent();
        }

        private void PwdBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if(DataContext != null)
            {
                (DataContext as Core.ViewModels.CreateProfileViewModel).Key = 
                    (sender as PasswordBox).Password;
            }
        }
    }
}
