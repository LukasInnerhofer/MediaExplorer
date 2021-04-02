using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Services
{
    class InvalidKeyException : Exception
    {
        public InvalidKeyException()
        {

        }

        public InvalidKeyException(string message) : base(message)
        {

        }
    }
}
