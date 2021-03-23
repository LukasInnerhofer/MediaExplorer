using System.ComponentModel;
using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using MediaExplorer.Core.ViewModels;

namespace MediaExplorer.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ProfileView.xaml
    /// </summary>
    [MvxViewFor(typeof(Core.ViewModels.ProfileViewModel))]
    public partial class ProfileView : MvxWpfView
    {
        public ProfileView()
        {
            InitializeComponent();
        }

        private void ListViewItem_PreviewGotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            ListView.SelectedItem = (sender as ListViewItem).DataContext;
        }
    }
}
