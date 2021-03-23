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
    [MvxViewFor(typeof(VirtualFolderViewModel))]
    public partial class VirtualFolderView : MvxWpfView
    {
        public VirtualFolderView() : base()
        {
            InitializeComponent();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextBox(DataContext as VirtualFolderViewModel);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch(e.PropertyName)
            {
                case nameof(VirtualFolderViewModel.IsNameReadOnly):
                    UpdateTextBox(sender as VirtualFolderViewModel);
                    break;
            }
        }

        private void MvxWpfView_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as VirtualFolderViewModel).PropertyChanged += OnViewModelPropertyChanged;
        }

        private void UpdateTextBox(VirtualFolderViewModel viewModel)
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
