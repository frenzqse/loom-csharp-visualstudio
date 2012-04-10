using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    class UIItem
    {
        public Item ItemModel { get; set; }
        public bool IsChecked { get; set; }

        public UIItem(Item i)
        {
            ItemModel = i;
        }
    }
}
