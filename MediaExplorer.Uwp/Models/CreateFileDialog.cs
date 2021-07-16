using MediaExplorer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace MediaExplorer.Uwp.Models
{
    class CreateFileDialog : ICreateFileDialog
    {
        FileSavePicker _picker;

        public string FileName { get; private set; }
        public bool RestoreDirectory { get { return false; } set { } }

        public IDictionary<string, IList<string>> Filter { get; private set; }

        public CreateFileDialog()
        {
            Filter = new Dictionary<string, IList<string>>();
        }

        public async Task<CreateFileDialogResult> ShowDialogAsync()
        {
            CreateFileDialogResult result = CreateFileDialogResult.None;
            _picker = new FileSavePicker();

            if (Filter.Count == 0)
            {
                throw new InvalidOperationException("Filter must not be empty");
            }
            else
            {
                foreach (KeyValuePair<string, IList<string>> filterEntry in Filter)
                {
                    _picker.FileTypeChoices.Add(filterEntry.Key, filterEntry.Value);
                }
            }

            StorageFile file = await _picker.PickSaveFileAsync();
            if (file != null)
            {
                FileName = file.Path;
                result = CreateFileDialogResult.Ok;
            }
            else
            {
                result = CreateFileDialogResult.Cancel;
            }

            return result;
        }
    }
}
