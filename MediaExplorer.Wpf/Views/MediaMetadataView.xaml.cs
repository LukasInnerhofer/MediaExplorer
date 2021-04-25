using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using MediaExplorer.Core.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MediaMetadataView.xaml
    /// </summary>
    [MvxViewFor(typeof(MediaMetadataViewModel))]
    public partial class MediaMetadataView : MvxWpfView
    {
        public MediaMetadataView()
        {
            InitializeComponent();
        }
    }
}
