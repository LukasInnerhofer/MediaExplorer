using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    public interface IOpenFolderDialog
    {
        string SelectedPath { get; }
        bool? ShowDialog();
    }
}
