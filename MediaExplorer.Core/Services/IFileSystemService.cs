using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    public interface IFileSystemService
    {
        Task CreateFileAsync(string path);
        Task<FileStream> OpenFileAsync(string path);

        Task<bool> FileExistsAsync(string path);
    }
}
