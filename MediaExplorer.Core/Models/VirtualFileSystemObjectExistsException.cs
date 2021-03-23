using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    public class VirtualFileSystemObjectExistsException : Exception
    {
        public string Name { get; private set; }

        public VirtualFileSystemObjectExistsException()
        {

        }

        public VirtualFileSystemObjectExistsException(string message) : base(message)
        {

        }

        public VirtualFileSystemObjectExistsException(string message, string name) : base(message)
        {
            Name = name;
        }
    }
}
