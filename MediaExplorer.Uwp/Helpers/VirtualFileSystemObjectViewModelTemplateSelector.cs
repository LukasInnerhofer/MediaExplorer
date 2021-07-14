using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MediaExplorer.Core.ViewModels;

namespace MediaExplorer.Uwp.Helpers
{
    class VirtualFileSystemObjectViewModelTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is VirtualAlbumFileViewModel)
            {
                return VirtualAlbumFileViewModelTemplate;
            }
            else
            {
                return VirtualFolderViewModelTemplate;
            }
        }

        public DataTemplate VirtualAlbumFileViewModelTemplate { get; set; }
        public DataTemplate VirtualFolderViewModelTemplate { get; set; }
    }
}
