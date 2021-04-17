using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MediaExplorer.Core.Services
{
    interface IHttpListenerService
    {
        void Register(string url, Action<HttpListenerContext> cb);
    }
}
