using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    public enum OpenFileDialogResult { Cancel, None, Ok };

    public interface IOpenFolderDialog
    {
        string SelectedPath { get; }
        bool? ShowDialog();
    }
}
