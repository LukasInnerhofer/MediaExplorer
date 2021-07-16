using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    public enum CreateFileDialogResult { None, Ok, Cancel };

    public interface ICreateFileDialog
    {
        string FileName { get; }
        bool RestoreDirectory { get; set; }
        IDictionary<string, IList<string>> Filter { get; }

        Task<CreateFileDialogResult> ShowDialogAsync();
    }
}
