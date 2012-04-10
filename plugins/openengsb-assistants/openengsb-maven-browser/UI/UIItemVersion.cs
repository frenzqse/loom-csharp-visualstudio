using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    class UIItemVersion
    {
        public ItemVersion ItemVersionModel { get; set; }
        public IList<UIItem> Items { get; set; }

        public UIItemVersion(ItemVersion version)
        {
            ItemVersionModel = version;
            Items = new List<UIItem>();
        }
    }
}
