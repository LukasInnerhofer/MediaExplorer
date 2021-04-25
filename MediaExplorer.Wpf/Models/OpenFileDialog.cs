using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Wpf.Models
{
    class OpenFileDialog : IOpenFileDialog
    {
        private System.Windows.Forms.OpenFileDialog _dialog;

        public string[] FileNames => _dialog.FileNames;

        public string FileName => _dialog.FileName;

        public string Filter 
        { 
            get { return _dialog.Filter; }
            set { _dialog.Filter = value; }
        }
        public string InitialDirectory 
        { 
            get { return _dialog.InitialDirectory; }
            set { _dialog.InitialDirectory = value; } 
        }
        public bool RestoreDirectory 
        { 
            get { return _dialog.RestoreDirectory; } 
            set { _dialog.RestoreDirectory = value;  }
        }
        public bool Multiselect
        {
            get { return _dialog.Multiselect; }
            set { _dialog.Multiselect = value; }
        }

        public OpenFileDialog()
        {
            _dialog = new System.Windows.Forms.OpenFileDialog();
        }

        public OpenFileDialogResult ShowDialog()
        {
            switch(_dialog.ShowDialog())
            {
                case DialogResult.None:
                    return OpenFileDialogResult.None;
                case DialogResult.OK:
                    return OpenFileDialogResult.Ok;
                case DialogResult.Cancel:
                default:
                    return OpenFileDialogResult.Cancel;
            }
        }
    }
}
