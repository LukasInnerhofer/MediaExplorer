using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public abstract class VirtualFileSystemObject
    {
        public string Name { get; protected set; }
        [field: NonSerialized]
        public bool IsNameReadOnly { get; set; }
        public VirtualFolder Parent { get; private set; }

        protected ObservableCollection<VirtualFileSystemObject> _children;
        public ReadOnlyObservableCollection<VirtualFileSystemObject> Children { get { return new ReadOnlyObservableCollection<VirtualFileSystemObject>(_children); } }

        protected List<VirtualFileSystemObject> Siblings
        {
            get
            {
                if(Parent == null)
                {
                    return new List<VirtualFileSystemObject>();
                }
                else
                {
                    return Parent._children.Except(new List<VirtualFileSystemObject>() { this }).ToList();
                }
            }
        }

        public VirtualFileSystemObject(string name, VirtualFolder parent)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"Parameter must not be empty.", name);
            }
            Name = name;
            Parent = parent;
            _children = new ObservableCollection<VirtualFileSystemObject>();
        }

        public void Rename(string newName)
        {
            if(string.IsNullOrEmpty(newName))
            {
                throw new ArgumentException("Parameter must not be empty", newName);
            }
            else if (Siblings.Where(x => x.Name == newName).Count() > 0)
            {
                throw new VirtualFileSystemObjectExistsException("An object with the given name already exists", newName);
            }
            Name = newName;
        }

        public virtual async Task InitializeNonSerializedMembers(object param)
        {

        }
    }
}
