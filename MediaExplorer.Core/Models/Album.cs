using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class Album
    {
        public string Name { get; private set; }
        
        [field: NonSerialized]
        public string FilePath { get; private set; }

        private List<MediaCollection> _mediaCollections;
        public IReadOnlyCollection<MediaCollection> MediaCollections { get { return _mediaCollections; } }

        public Album()
        {
            Name = string.Empty;
            FilePath = string.Empty;
            _mediaCollections = new List<MediaCollection>();
        }

        public static Album FromBasePath(string basePath)
        {
            var album = new Album();

            album.Name = basePath.Split(System.IO.Path.DirectorySeparatorChar).Last();
            foreach(string file in System.IO.Directory.EnumerateFiles(basePath))
            {

            }

            return album;
        }
    }
}
