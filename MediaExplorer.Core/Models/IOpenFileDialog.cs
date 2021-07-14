using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    public enum OpenFileDialogResult { None, Ok, Cancel };

    public interface IOpenFileDialog
    {
        IReadOnlyList<string> FileNames { get; }
        string FileName { get; }
        bool RestoreDirectory { get; set; }
        bool Multiselect { get; set; }
        IList<string> Filter { get; }

        Task<OpenFileDialogResult> ShowDialogAsync();
    }
}
