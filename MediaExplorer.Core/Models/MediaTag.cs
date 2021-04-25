using System;
using System.Collections.Generic;
using System.Text;

namespace MediaExplorer.Core.Models
{
    [Serializable]
    public class MediaTag
    {
        public string Text { get; private set; }

        public MediaTag(string text)
        {
            Text = text;
        }
    }
}
