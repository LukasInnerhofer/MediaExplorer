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
    public class MediaCollectionViewModel : MvxViewModel
    {
        private int _it;
        private int It
        {
            get { return _it; }
            set
            {
                _it = value;
                RaisePropertyChanged(nameof(MediaCollection));
            }
        }

        private string _albumName;
        private byte[] _key;

        private MediaCollection _mediaCollection;
        private MediaCollection MediaCollection
        {
            get { return _mediaCollection; }
            set
            {
                _mediaCollection = value;
                RaisePropertyChanged(nameof(Media));
            }
        }

        private Media Media
        {
            get
            {
                if (MediaCollection.Media.Count <= It)
                {
                    return null;
                }
                else
                {
                    return MediaCollection.Media[It];
                }
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
                    $"{_albumName}/{MediaCollection.Name}/{Media.Path.Split(Path.DirectorySeparatorChar).Last()}/",
                    httpMediaRequest);
                return $"http://127.0.0.1:12345/{_albumName}/{MediaCollection.Name}/{Media.Path.Split(Path.DirectorySeparatorChar).Last()}/";
            }
        }

        public MediaCollectionViewModel(MediaCollection mediaCollection, string albumName, byte[] key)
        {
            MediaCollection = mediaCollection;
            _albumName = albumName;
            _key = key;
        }

        private void httpMediaRequest(HttpListenerContext context)
        {
            Stream responseStream = context.Response.OutputStream;
            using (var fileStream = new FileStream(Media.Path, FileMode.Open))
            {
                Mvx.IoCProvider.Resolve<ICryptographyService>().DecryptAsync(fileStream, responseStream, _key).Wait();
            }
            responseStream.Close();
        }
    }
}
