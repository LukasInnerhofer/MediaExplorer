﻿using MediaExplorer.Core.ViewModels;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
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
    /// Interaction logic for MediaCollectionView.xaml
    /// </summary>
    [MvxViewFor(typeof(MediaViewModel))]
    public partial class MediaView : MvxWpfView
    {
        public MediaView() : base()
        {
            InitializeComponent();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            MediaElement.Position = new TimeSpan(0, 0, 0);
            MediaElement.Play();
        }
    }
}
