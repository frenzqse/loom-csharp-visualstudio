using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    class Item
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public Item(string name, string url)
        {
            Name = name;
            Url = url;
        }
    }
}
