using System.Windows.Controls;
using MvvmCross.Platforms.Wpf.Views;
using MediaExplorer.Core.ViewModels;
using MvvmCross.ViewModels;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

namespace MediaExplorer.Wpf.Views
{
    /// <summary>
    /// Interaction logic for VirtualFolderView.xaml
    /// </summary>
    [MvxViewFor(typeof(VirtualAlbumFileViewModel))]
    public partial class VirtualAlbumFileView : MvxWpfView
    {
        public VirtualAlbumFileView() : base()
        {
            InitializeComponent();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextBox(DataContext as VirtualAlbumFileViewModel);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(VirtualFolderViewModel.IsNameReadOnly):
                    UpdateTextBox(sender as VirtualAlbumFileViewModel);
                    break;
            }
        }

        private void MvxWpfView_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as VirtualAlbumFileViewModel).PropertyChanged += OnViewModelPropertyChanged;
        }

        private void UpdateTextBox(VirtualAlbumFileViewModel viewModel)
        {
            if (viewModel.IsNameReadOnly)
            {
                TextBox.SelectionLength = 0;
            }
            else
            {
                Keyboard.Focus(TextBox);
                TextBox.SelectAll();
            }
        }
    }
}
