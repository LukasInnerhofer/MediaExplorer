using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    public enum OpenFileDialogResult { None, Ok, Cancel };

    public interface IOpenFileDialog
    {
        string[] FileNames { get; }
        string FileName { get; }
        string Filter { get; set; }
        string InitialDirectory { get; set; }
        bool RestoreDirectory { get; set; }
        bool Multiselect { get; set; }

        OpenFileDialogResult ShowDialog();
    }
}
