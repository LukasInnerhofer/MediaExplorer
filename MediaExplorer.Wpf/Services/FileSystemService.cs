using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaExplorer.Core.Services;

namespace MediaExplorer.Wpf.Services
{
    class FileSystemService : IFileSystemService
    {
        public Task CreateFileAsync(string path)
        {
            File.Create(path).Close();
            return Task.CompletedTask;
        }

        public Task<bool> FileExistsAsync(string path)
        {
            return Task.FromResult(File.Exists(path));
        }

        public Task<FileStream> OpenFileAsync(string path)
        {
            return Task.FromResult(new FileStream(path, FileMode.Open));
        }
    }
}
