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

            while (!File.Exists(_realPath))
            {
                MessageBoxResult result = Mvx.IoCProvider.Resolve<IMessageBoxService>().Show(
                    $"Album {Name} does not exist. Would you like to update its path?",
                    "Album not found",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.Yes);
                if(result == MessageBoxResult.Yes)
                {
                    IOpenFileDialog dialog = Mvx.IoCProvider.Resolve<IFileDialogService>().GetOpenFileDialog();
                    dialog.RestoreDirectory = true;
                    if(dialog.ShowDialog() == OpenFileDialogResult.Ok)
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
                /*
                string outName = "";
                foreach (string part in Path.GetDirectoryName(_realPath).Split(Path.DirectorySeparatorChar))
                {
                    outName += part;
                }
                using (var os = new MemoryStream())//new FileStream(outName, FileMode.Create))
                {
                    await Mvx.IoCProvider.Resolve<ICryptographyService>().DecryptAsync(fs, os, _key);
                    os.Seek(0, SeekOrigin.Begin);
                    var buffer = new byte[os.Length];
                    os.Read(buffer, 0, buffer.Length);
                    string text = Encoding.UTF8.GetString(buffer);
                    text = text.Replace("System.Collections.Generic.List`1[[MediaExplorer.Core.Models.Media,", "System.Collections.ObjectModel.ObservableCollection`1[[MediaExplorer.Core.Models.Media,");
                    var outBuffer = Encoding.UTF8.GetBytes(text);
                    using(var ofs = new FileStream(outName, FileMode.Create))
                    {
                        ofs.Write(outBuffer, 0, outBuffer.Length);
                        ofs.Seek(0, SeekOrigin.Begin);
                        using(var eofs = new FileStream(outName + "encrypted", FileMode.Create))
                        {
                            await Mvx.IoCProvider.Resolve<ICryptographyService>().EncryptAsync(ofs, eofs, _key);
                        }
                    }
                }*/
                Album = await Mvx.IoCProvider.Resolve<ICryptographyService>().DeserializeAsync<Album>(fs, _key);
                Album.InitializeNonSerializedMembers(_key, _realPath);
            }
        }
    }
}
