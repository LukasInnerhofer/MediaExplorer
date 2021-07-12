﻿using System;
using System.Collections.Generic;
using System.Text;
using MediaExplorer.Core.Models;

namespace MediaExplorer.Core.Services
{
    public interface IFileDialogService
    {
        ICreateFileDialog GetCreateFileDialog();
        IOpenFolderDialog GetOpenFolderDialog();
        IOpenFileDialog GetOpenFileDialog();
    }
}
