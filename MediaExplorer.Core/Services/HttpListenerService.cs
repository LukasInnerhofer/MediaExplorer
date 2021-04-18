﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Services
{
    class HttpListenerService : IHttpListenerService
    {
        private List<Tuple<string, Action<HttpListenerContext>>> _observers;

        HttpListener _httpListener;
        Thread _httpThread;
        bool _exit = false;

        public HttpListenerService()
        {
            _observers = new List<Tuple<string, Action<HttpListenerContext>>>();

            var threadStart = new ThreadStart(delegate ()
            {
                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add("http://127.0.0.1:12345/");
                _httpListener.Start();
                while (!_exit)
                {
                    HttpListenerContext context = _httpListener.GetContext();
                    lock (_observers)
                    {
                        string relativePath = context.Request.Url.AbsoluteUri.Replace("http://127.0.0.1:12345/", "");

                        if(relativePath == "Exit")
                        {
                            context.Response.OutputStream.Close();
                            _httpListener.Stop();
                            _httpListener.Close();
                            break;
                        }

                        foreach (Tuple<string, Action<HttpListenerContext>> observer in _observers)
                        {
                            if (relativePath.Length >= observer.Item1.Length)
                            {
                                if (relativePath.Substring(0, observer.Item1.Length) == observer.Item1)
                                {
                                    observer.Item2.Invoke(context);
                                }
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
            lock(_observers)
            {
                _observers.Add(new Tuple<string, Action<HttpListenerContext>>(url, cb));
            }
        }

        public void Exit()
        {
            _exit = true;
            if (_httpListener != null)
            {
                HttpClient client = new HttpClient();
                client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "http://127.0.0.1:12345/Exit")).Wait();
                _httpThread.Join();
            }
        }
    }
}
