using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class Profile
    {
        public VirtualFolder RootFolder { get; private set; }
        [field: NonSerialized]
        public byte[] KeyHash { get; set; }
        [field: NonSerialized]
        public string Path { get; private set; }

        public Profile()
        {
            RootFolder = new VirtualFolder("root", null);
        }

        public async Task InitializeNonSerializedMembers(byte[] keyHash, string path)
        {
            KeyHash = keyHash;
            Path = path;
            await RootFolder.InitializeNonSerializedMembers(keyHash);
        }
    }
}
