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

        public IReadOnlyList<string> FileNames => _dialog.FileNames;

        public string FileName => _dialog.FileName;

        public IDictionary<string, IList<string>> Filter { get; private set; }
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
            Filter = new Dictionary<string, IList<string>>();
        }

        public Task<OpenFileDialogResult> ShowDialogAsync()
        {
            if (Filter.Count == 0)
            {
                throw new InvalidOperationException("Filter must not be empty");
            }
            else
            {
                string filter = "";
                foreach (KeyValuePair<string, IList<string>> filterEntry in Filter)
                {
                    string fileTypes = "";

                    foreach (string fileType in filterEntry.Value)
                    {
                        fileTypes += $"*{fileType};";
                    }
                    fileTypes = fileTypes.Remove(fileTypes.Length - 1);

                    filter += $"{filterEntry.Key} |{fileTypes}|";
                }
                _dialog.Filter = filter.Remove(filter.Length - 1);
            }
            return Task.FromResult(ConvertResult(_dialog.ShowDialog()));
        }

        private OpenFileDialogResult ConvertResult(DialogResult result)
        {
            switch (result)
            {
                default:
                case DialogResult.None:
                    return OpenFileDialogResult.None;
                case DialogResult.OK:
                    return OpenFileDialogResult.Ok;
                case DialogResult.Cancel:
                    return OpenFileDialogResult.Cancel;
            }
        }
    }
}
