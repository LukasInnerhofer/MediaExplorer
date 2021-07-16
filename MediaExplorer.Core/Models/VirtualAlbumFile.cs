using MediaExplorer.Core.Services;
using MvvmCross;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class VirtualAlbumFile : VirtualFileSystemObject
    {
        private string _realPath;

        [NonSerialized]
        byte[] _key;

        [field: NonSerialized]
        public Album Album { get; set; }

        public VirtualAlbumFile(Album album, VirtualFolder parent) : base(album.Name, parent)
        {
            Album = album;
            _realPath = Album.FilePath;
            _key = Album.Key;
        }

        public override async Task InitializeNonSerializedMembers(object param)
        {
            _key = param as byte[];

            while (!await Mvx.IoCProvider.Resolve<IFileSystemService>().FileExistsAsync(_realPath))
            {
                MessageBoxResult result = await Mvx.IoCProvider.Resolve<IMessageBoxService>().ShowAsync(
                    $"Album {Name} does not exist. Would you like to update its path?",
                    "Album not found",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);
                if(result == MessageBoxResult.Yes)
                {
                    IOpenFileDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetOpenFileDialog();
                    dialog.RestoreDirectory = true;
                    if(await dialog.ShowDialogAsync() == OpenFileDialogResult.Ok)
                    {
                        _realPath = dialog.FileName;
                    }
                }
                else
                {
                    return;
                }
            }

            using (var fs = new FileStream(_realPath, FileMode.Open))
            {
                Album = await Mvx.IoCProvider.Resolve<ICryptographyService>().DeserializeAsync<Album>(fs, _key);
                Album.InitializeNonSerializedMembers(_key, _realPath);
            }
        }
    }
}
