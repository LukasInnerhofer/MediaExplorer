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

        private void ListView_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch(e.Key)
            {
                case System.Windows.Input.Key.Return:
                    (DataContext as ProfileViewModel).OpenCommand?.Execute();
                    break;
            }
        }

        private void ListView_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            (DataContext as ProfileViewModel).OpenCommand?.Execute();
        }
    }
}
