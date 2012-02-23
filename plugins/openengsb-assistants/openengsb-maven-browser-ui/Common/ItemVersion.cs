using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    class ItemVersion
    {
        public string Id { get; set; }
        public IList<Item> Items { get; set; }

        public ItemVersion(string id)
        {
            Id = id;
            Items = new List<Item>();
        }
    }
}
