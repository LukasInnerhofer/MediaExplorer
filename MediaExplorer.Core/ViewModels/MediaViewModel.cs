using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaExplorer.Core.ViewModels
{
    public class MediaViewModel : MvxViewModel
    {
        private string _albumName;
        private byte[] _key;
        private MediaCollection _mediaCollection;

        private Media _media;
        public Media Media
        {
            get
            {
                return _media;
            }
            private set
            {
                SetProperty(ref _media, value);
                RaisePropertyChanged(nameof(Metadata));
            }
        }

        public string MediaUrl
        {
            get
            {
                if(Media == null)
                {
                    return "placeholder.png";
                }

                Mvx.IoCProvider.Resolve<IHttpListenerService>().Register(
                    $"{_albumName}/{_mediaCollection.Name}/{Media.Path.Split(Path.DirectorySeparatorChar).Last()}/",
                    HttpMediaRequest);
                return $"http://127.0.0.1:12345/{_albumName}/{_mediaCollection.Name}/{Media.Path.Split(Path.DirectorySeparatorChar).Last()}/";
            }
        }

        public MediaMetadataViewModel Metadata { get { return new MediaMetadataViewModel(Media.Metadata); } }

        public MediaViewModel(Media media, MediaCollection mediaCollection, string albumName, byte[] key)
        {
            _mediaCollection = mediaCollection;
            _albumName = albumName;
            _key = key;
            Media = media;
        }

        private void HttpMediaRequest(HttpListenerContext context)
        {
            Stream responseStream = context.Response.OutputStream;
            using (var fileStream = new FileStream(Media.Path, FileMode.Open))
            {
                Mvx.IoCProvider.Resolve<ICryptographyService>().DecryptAsync(fileStream, responseStream, _key).Wait();
            }
            //try
            {
                responseStream.Close();
            }
           // catch(Exception)
           // { }
        }
    }
}
