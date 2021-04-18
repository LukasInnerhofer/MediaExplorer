using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MediaExplorer.Core.Services
{
    public interface IHttpListenerService
    {
        void Register(string url, Action<HttpListenerContext> cb);
        void Exit();
    }
}
