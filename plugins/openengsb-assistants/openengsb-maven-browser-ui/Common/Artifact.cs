using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    class Artifact
    {
        public string Id { get; set; }
        public IList<ItemVersion> Versions { get; set; }

        public Artifact(string id)
        {
            Id = id;
            Versions = new List<ItemVersion>();
        }
    }
}
