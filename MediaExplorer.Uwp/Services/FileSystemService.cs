using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaExplorer.Core.Services;
using Windows.Storage;
using Microsoft.Win32.SafeHandles;

namespace MediaExplorer.Uwp.Services
{
    public class FileSystemService : IFileSystemService
    {
        public async Task CreateFileAsync(string path)
        {
            string fileName = path.Split(Path.DirectorySeparatorChar).Last();
            StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(path.Replace(fileName, ""));
            StorageFile file = await folder.GetFileAsync(fileName);
            if (file != null)
            {
                await file.DeleteAsync();
            }
            await folder.CreateFileAsync(fileName);
        }

        public async Task<FileStream> OpenFileAsync(string path)
        {
            return new FileStream(WindowsRuntimeStorageExtensions.CreateSafeFileHandle(await StorageFile.GetFileFromPathAsync(path)), FileAccess.ReadWrite);
        }

        public async Task<bool> FileExists(string path)
        {
            return await StorageFile.GetFileFromPathAsync(path) != null;
        }
    }
}
