using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    public class HttpObserver : IHttpObserver
    {
        public string Url { get; private set; }
        public Action<HttpListenerContext> Callback { get; private set; }

        public HttpObserver(string url, Action<HttpListenerContext> callback)
        {
            Url = url;
            Callback = callback;
        }
    }

    class HttpListenerService : IHttpListenerService
    {
        private HashSet<IHttpObserver> _observers;

        private HttpListener _httpListener;
        private Thread _httpThread;
        private bool _exit = false;

        public HttpListenerService()
        {
            _observers = new HashSet<IHttpObserver>();

            var threadStart = new ThreadStart(delegate ()
            {
                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add("http://127.0.0.1:12345/");
                _httpListener.Start();
                while (!_exit)
                {
                    HttpListenerContext context;
                    try
                    {
                       context = _httpListener.GetContext();
                    }
                    catch(HttpListenerException _)
                    {
                        break;
                    }
                    
                    string relativePath = context.Request.Url.AbsoluteUri.Replace("http://127.0.0.1:12345/", "");

                    lock (_observers)
                    {
                        foreach (HttpObserver observer in _observers)
                        {
                            if (relativePath.Length >= observer.Url.Length)
                            {
                                if (relativePath.Substring(0, observer.Url.Length) == observer.Url)
                                {
                                    observer.Callback.Invoke(context);
                                }
                            }
                        }
                    }
                }
            });
            _httpThread = new Thread(threadStart);
            _httpThread.Start();
        }

        public IHttpObserver CreateObserver(string url, Action<HttpListenerContext> callback)
        {
            return new HttpObserver(url, callback);
        }

        public void Register(IHttpObserver observer)
        {
            lock(_observers)
            {
                _observers.Add(observer);
            }
        }

        public void Unregister(IHttpObserver observer)
        {
            // TODO: This sometimes blocks for a long time even though no one else has acquired the lock.
            // lock(_observers)
            {
                _observers.Remove(observer);
            }
        }

        public void Exit()
        {
            _exit = true;
            if (_httpListener != null)
            {
                _httpListener.Abort();
                _httpThread.Join();
            }
        }
    }
}
