using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Testing
{
    class DataGenerator
    {
        private int _nextItem;
        private int _nextVersion;
        private int _nextArtifact;

        public DataGenerator()
        {
            _nextArtifact = 0;
            _nextVersion = 0;
            _nextItem = 0;
        }

        public Item NextItem(ItemVersion version)
        {
            return new Item("Item " + _nextItem, "https://maven.org?item=" + _nextItem++, version);
        }
        
        public ItemVersion NextVersion(Artifact artifact)
        {
            return new ItemVersion("Version: " + _nextVersion++, artifact);
        }

        public Artifact NextArtifact()
        {
            return new Artifact("org.openengsb.domain", "Artifact: " + _nextArtifact++);
        }
    }
}
