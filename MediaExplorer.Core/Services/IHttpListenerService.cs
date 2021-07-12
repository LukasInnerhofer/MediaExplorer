using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MediaExplorer.Core.Services
{
    public interface IHttpObserver
    {
        string Url { get; }
        Action<HttpListenerContext> Callback { get; }
    }

    public interface IHttpListenerService
    {
        IHttpObserver CreateObserver(string url, Action<HttpListenerContext> callback);
        void Register(IHttpObserver observer);
        void Unregister(IHttpObserver observer);
        void Exit();
    }
}
