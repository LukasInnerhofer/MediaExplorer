using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    public enum CreateFileDialogResult { None, Ok, Cancel };

    public interface ICreateFileDialog
    {
        IReadOnlyList<string> FileNames { get; }
        string FileName { get; }
        bool RestoreDirectory { get; set; }
        IList<string> Filter { get; }

        Task<CreateFileDialogResult> ShowDialog();
    }
}
