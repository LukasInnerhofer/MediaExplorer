using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaExplorer.Core.Models;
using Ookii.Dialogs.Wpf;

namespace MediaExplorer.Wpf.Models
{
    class OpenFolderDialog : IOpenFolderDialog
    {
        private VistaFolderBrowserDialog _dialog;

        public string SelectedPath 
        { 
            get
            {
                return _dialog.SelectedPath;
            }
        }

        public bool? ShowDialog()
        {
            return _dialog.ShowDialog();
        }

        public OpenFolderDialog()
        {
            _dialog = new VistaFolderBrowserDialog();
        }
    }
}
