using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class Profile
    {
        public VirtualFolder RootFolder { get; private set; }
        [field: NonSerialized]
        public byte[] KeyHash { get; set; }

        public Profile()
        {
            RootFolder = new VirtualFolder("root", null);
        }
    }
}
