using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Core.Services
{
    public interface IFileDialogService
    {
        IOpenFolderDialog GetOpenFolderDialog();
        IOpenFileDialog GetOpenFileDialog();
    }
}
