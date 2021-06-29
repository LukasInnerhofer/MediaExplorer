using MediaExplorer.Core.ViewModels;
using MvvmCross.Commands;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for AlbumIterativeView.xaml
    /// </summary>
    [MvxViewFor(typeof(IterativeAlbumViewModel))]
    public partial class IterativeAlbumView : MvxWpfView
    {
        public IterativeAlbumView() : base()
        {
            InitializeComponent();
            Loaded += IterativeAlbumView_Loaded;
            Unloaded += IterativeAlbumView_Unloaded;
        }

        private void IterativeAlbumView_Unloaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            foreach (InputBinding inputBinding in InputBindings)
            {
                window.InputBindings.Remove(inputBinding);
            }
        }

        private void IterativeAlbumView_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            foreach (InputBinding inputBinding in InputBindings)
            {
                window.InputBindings.Add(inputBinding);
            }
        }
    }
}
