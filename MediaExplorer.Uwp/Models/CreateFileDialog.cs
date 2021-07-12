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

        private List<string> _fileNames;
        public IReadOnlyList<string> FileNames { get { return _fileNames; } }

        public string FileName { get { return FileNames[0]; } }
        public bool RestoreDirectory { get { return false; } set { } }

        private List<string> _filter;
        public IList<string> Filter { get { return _filter; } }

        public CreateFileDialog()
        {
            _filter = new List<string>();
        }

        public async Task<CreateFileDialogResult> ShowDialog()
        {
            _fileNames = new List<string>();
            CreateFileDialogResult result = CreateFileDialogResult.None;
            _picker = new FileSavePicker();

            if (_filter.Count == 0)
            {
                _picker.FileTypeChoices.Add("All files", new List<string>() { ".media_explorer_profile" });
            }
            else
            {
                foreach (string filterEntry in Filter)
                {
                    _picker.FileTypeChoices.Add(filterEntry, new List<string>() { filterEntry });
                }
            }

            StorageFile file = await _picker.PickSaveFileAsync();
            if (file != null)
            {
                _fileNames.Add(file.Path);
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
