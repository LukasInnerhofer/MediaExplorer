using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class VirtualFolder : VirtualFileSystemObject
    {
        public VirtualFolder(string name, VirtualFolder parent) : base(name, parent)
        {
            
        }

        public bool AddChild(VirtualFileSystemObject child)
        {
            if(_children.Any(x => x.Name == child.Name))
            {
                return false;
            }
            _children.Add(child);
            return true;
        }

        public override async Task InitializeNonSerializedMembers(object param)
        {
            foreach(VirtualFileSystemObject child in Children)
            {
                await child.InitializeNonSerializedMembers(param);
            }
        }
    }
}
