using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Wpf.Models
{
    class CreateFileDialog : ICreateFileDialog
    {
        private SaveFileDialog _dialog;

        public string FileName => _dialog.FileName;

        public bool RestoreDirectory
        {
            get
            {
                return _dialog.RestoreDirectory;
            }
            set
            {
                _dialog.RestoreDirectory = value;
            }
        }

        public IDictionary<string, IList<string>> Filter { get; private set; }

        public CreateFileDialog()
        {
            _dialog = new SaveFileDialog();
            Filter = new Dictionary<string, IList<string>>();
        }

        public Task<CreateFileDialogResult> ShowDialogAsync()
        {
            if (Filter.Count == 0)
            {
                throw new InvalidOperationException("Filter must not be empty");
            }
            else
            {
                foreach (KeyValuePair<string, IList<string>> filterEntry in Filter)
                {
                    string fileTypes = "";

                    foreach (string fileType in filterEntry.Value)
                    {
                        fileTypes += $"{fileType};";
                    }
                    fileTypes = fileTypes.Remove(fileTypes.Length - 1);

                    _dialog.Filter += $"{filterEntry.Key} ({fileTypes})|{fileTypes}|";
                }
                _dialog.Filter = _dialog.Filter.Remove(_dialog.Filter.Length - 1);
            }
            return Task.FromResult(ConvertResult(_dialog.ShowDialog()));
        }

        private CreateFileDialogResult ConvertResult(DialogResult result)
        {
            switch (result)
            {
                default:
                case DialogResult.None:
                    return CreateFileDialogResult.None;
                case DialogResult.OK:
                    return CreateFileDialogResult.Ok;
                case DialogResult.Cancel:
                    return CreateFileDialogResult.Cancel;
            }
        }
    }
}
