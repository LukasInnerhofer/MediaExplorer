﻿using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Services;
using MediaExplorer.Core.Models;
using MediaExplorer.Wpf.Models;

namespace MediaExplorer.Wpf.Services
{
    public class FileDialogService : IFileDialogService
    {
        public IOpenFolderDialog GetOpenFolderDialog()
        {
            return new OpenFolderDialog();
        }

        public IOpenFileDialog GetOpenFileDialog()
        {
            return new OpenFileDialog();
        }
    }
}
