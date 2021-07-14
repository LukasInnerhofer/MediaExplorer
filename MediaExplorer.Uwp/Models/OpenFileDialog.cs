using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using MediaExplorer.Core.Models;
using Windows.Storage;

namespace MediaExplorer.Uwp.Models
{
    public class OpenFileDialog : IOpenFileDialog
    {
        FileOpenPicker _picker;

        private List<string> _fileNames;
        public IReadOnlyList<string> FileNames { get { return _fileNames; } }

        public string FileName { get { return FileNames[0]; } }
        public bool RestoreDirectory { get { return false; } set { } }
        public bool Multiselect { get; set; }

        private List<string> _filter;
        public IList<string> Filter { get { return _filter; } }

        public OpenFileDialog()
        {
            _filter = new List<string>();
        }

        public async Task<OpenFileDialogResult> ShowDialogAsync()
        {
            _fileNames = new List<string>();
            OpenFileDialogResult result = OpenFileDialogResult.None;
            _picker = new FileOpenPicker();
            _picker.ViewMode = PickerViewMode.Thumbnail;
            
            if (_filter.Count == 0)
            {
                _picker.FileTypeFilter.Add("*");
            }
            else
            {
                foreach (string filterEntry in Filter)
                {
                    _picker.FileTypeFilter.Add(filterEntry);
                }
            }

            if (Multiselect)
            {
                IReadOnlyList<StorageFile> files = await _picker.PickMultipleFilesAsync();
                if (files.Count > 0)
                {
                    foreach (StorageFile file in files)
                    {
                        _fileNames.Add(file.Path);
                    }
                    result = OpenFileDialogResult.Ok;
                }
                else
                {
                    result = OpenFileDialogResult.Cancel;
                }
            }
            else
            {
                StorageFile file = await _picker.PickSingleFileAsync();
                if (file != null)
                {
                    _fileNames.Add(file.Path);
                    result = OpenFileDialogResult.Ok;
                }
                else
                {
                    result = OpenFileDialogResult.Cancel;
                }
            }

            return result;
        }
    }
}
