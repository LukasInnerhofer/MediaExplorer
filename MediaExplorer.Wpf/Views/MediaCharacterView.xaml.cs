using MediaExplorer.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaExplorer.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MediaCharacterView.xaml
    /// </summary>
    [MvxViewFor(typeof(MediaCharacterViewModel))]
    public partial class MediaCharacterView : MvxWpfView
    {
        public MediaCharacterView()
        {
            InitializeComponent();
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateTextBox(DataContext as MediaCharacterViewModel);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MediaCharacterViewModel.IsNameReadOnly):
                    UpdateTextBox(sender as MediaCharacterViewModel);
                    break;
            }
        }

        private void MvxWpfView_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as MediaCharacterViewModel).PropertyChanged += OnViewModelPropertyChanged;
        }

        private void UpdateTextBox(MediaCharacterViewModel viewModel)
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
