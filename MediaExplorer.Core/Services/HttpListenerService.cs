using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace MediaExplorer.Core.Services
{
    class HttpListenerService : IHttpListenerService
    {
        private List<Tuple<string, Action<HttpListenerContext>>> _observers;

        HttpListener _httpListener;
        Thread _httpThread;

        public HttpListenerService()
        {
            _observers = new List<Tuple<string, Action<HttpListenerContext>>>();

            var threadStart = new ThreadStart(delegate ()
            {
                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add("http://127.0.0.1:12345/");
                _httpListener.Start();
                while (true)
                {
                    HttpListenerContext context = _httpListener.GetContext();
                    foreach(Tuple<string, Action<HttpListenerContext>> observer in _observers)
                    {
                        string relativePath = context.Request.Url.AbsoluteUri.Replace("http://127.0.0.1:12345/", "");
                        if(relativePath.Length >= observer.Item1.Length)
                        {
                            if (relativePath.Substring(0, observer.Item1.Length) == observer.Item1)
                            {
                                observer.Item2.Invoke(context);
                            }
                        }
                    }
                }
            });
            _httpThread = new Thread(threadStart);
            _httpThread.Start();
        }

        public void Register(string url, Action<HttpListenerContext> cb)
        {
            _observers.Add(new Tuple<string, Action<HttpListenerContext>>(url, cb));
        }
    }
}
