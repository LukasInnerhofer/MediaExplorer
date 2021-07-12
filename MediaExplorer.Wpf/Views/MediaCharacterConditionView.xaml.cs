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
using MediaExplorer.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;

namespace MediaExplorer.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MediaCharacterConditionView.xaml
    /// </summary>
    [MvxViewFor(typeof(MediaCharacterConditionViewModel))]
    public partial class MediaCharacterConditionView : MvxWpfView
    {
        public MediaCharacterConditionView()
        {
            InitializeComponent();
        }
    }
}
