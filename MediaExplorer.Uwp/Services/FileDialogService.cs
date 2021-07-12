using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MediaExplorer.Uwp.Models;

namespace MediaExplorer.Uwp.Services
{
    class FileDialogService : IFileDialogService
    {
        public ICreateFileDialog GetCreateFileDialog()
        {
            return new CreateFileDialog();
        }

        public IOpenFileDialog GetOpenFileDialog()
        {
            return new OpenFileDialog();
        }

        public IOpenFolderDialog GetOpenFolderDialog()
        {
            throw new NotImplementedException();
        }
    }
}
