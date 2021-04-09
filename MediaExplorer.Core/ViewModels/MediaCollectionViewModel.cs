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

namespace MediaExplorer.Core.ViewModels
{
    public class MediaCollectionViewModel : MvxViewModel<Tuple<MediaCollection, string, byte[]>>
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

                string url = $"http://127.0.0.1:12345/{_albumName}/{MediaCollection.Name}/{Media.Path.Split(Path.DirectorySeparatorChar).Last()}/";

                if(_httpThread != null && _httpThread.IsAlive)
                {
                    _httpThread.Join();
                }
                var threadStart = new ThreadStart(delegate ()
                {
                    _httpListener = new HttpListener();
                    _httpListener.Prefixes.Add(url);
                    _httpListener.Start();
                    HttpListenerContext context = _httpListener.GetContext();
                    Stream responseStream = context.Response.OutputStream;
                    using(var fileStream = new FileStream(Media.Path, FileMode.Open))
                    {
                        Mvx.IoCProvider.Resolve<ICryptographyService>().DecryptAsync(fileStream, responseStream, _key).Wait();
                    }
                    responseStream.Close();
                    _httpListener.Stop();
                });
                _httpThread = new Thread(threadStart);
                _httpThread.Start();

                return url;
            }
        }

        private HttpListener _httpListener;
        private Thread _httpThread;

        public override void Prepare(Tuple<MediaCollection, string, byte[]> parameter)
        {
            MediaCollection = parameter.Item1;
            _albumName = parameter.Item2;
            _key = parameter.Item3;
        }
    }
}
