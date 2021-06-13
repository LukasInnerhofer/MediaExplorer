using MediaExplorer.Core.Models;
using MediaExplorer.Core.Services;
using MvvmCross;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        bool _done;
        public string MediaUrl
        {
            get
            {
                if(!_done)
                {
                    if(_thread == null)
                    {
                        _thread = new Thread(new ThreadStart(() =>
                        {
                            _stream = new MemoryStream();
                            using (var fileStream = new FileStream(Media.Path, FileMode.Open))
                            {
                                Mvx.IoCProvider.Resolve<ICryptographyService>().DecryptAsync(fileStream, _stream, _key).Wait();
                            }
                            _stream.Seek(0, SeekOrigin.Begin);
                            _done = true;
                            RaisePropertyChanged(nameof(MediaUrl));
                        }));
                        _thread.Start();
                    }

                    return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "placeholder.bmp";
                }
                

                string url = $"{_albumName}/{_mediaCollection.Name}/{Media.Path.Split(Path.DirectorySeparatorChar).Last()}/";
                if(_observer == null)
                {
                    _observer = Mvx.IoCProvider.Resolve<IHttpListenerService>().CreateObserver(url, HttpMediaRequest);
                    Mvx.IoCProvider.Resolve<IHttpListenerService>().Register(_observer);
                }
                
                return $"http://127.0.0.1:12345/{url}";
            }
        }

        public MediaMetadataViewModel Metadata { get { return new MediaMetadataViewModel(Media.Metadata, _allTags, _allCharacterTags, _allCharacterNames); } }

        private ReadOnlyObservableCollection<string> _allTags;
        private ReadOnlyObservableCollection<string> _allCharacterTags;
        private ReadOnlyObservableCollection<string> _allCharacterNames;

        private IHttpObserver _observer;
        private MemoryStream _stream;
        private Thread _thread;

        public MediaViewModel(
            Media media, 
            MediaCollection mediaCollection, 
            string albumName, byte[] key, 
            ReadOnlyObservableCollection<string> allTags,
            ReadOnlyObservableCollection<string> allCharacterTags,
            ReadOnlyObservableCollection<string> allCharacterNames)
        {
            _mediaCollection = mediaCollection;
            _albumName = albumName;
            _key = key;
            _stream = null;
            _observer = null;
            _done = false;
            _allTags = allTags;
            _allCharacterTags = allCharacterTags;
            _allCharacterNames = allCharacterNames;
            Media = media;
        }

        public void Close()
        {
            Mvx.IoCProvider.Resolve<IHttpListenerService>().Unregister(_observer);
            _observer = null;
        }

        private void HttpMediaRequest(HttpListenerContext context)
        {
            Stream responseStream = context.Response.OutputStream;

            try
            {
                _thread.Join();
                _stream.CopyTo(responseStream);
                responseStream.Close();
            }
            catch(Exception)
            {
                // TODO: WPF sometimes makes a bunch of requests for one file. 
                // Some of which cause an exception.
            }
            
            _stream.Seek(0, SeekOrigin.Begin);
        }
    }
}
