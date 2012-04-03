using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    public class WizardConfiguration
    {
        public string Path { get; set; }
        public IList<Item> Items { get; set; }

        public WizardConfiguration()
        {
            Path = ".";
            Items = new List<Item>();
        }
    }
}
