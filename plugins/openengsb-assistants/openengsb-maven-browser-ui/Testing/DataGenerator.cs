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

        public Item NextItem()
        {
            return new Item("Item " + _nextItem, "https://maven.org?item=" + _nextItem++);
        }
        
        public ItemVersion NextVersion()
        {
            return new ItemVersion("Version: " + _nextVersion++);
        }

        public Artifact NextArtifact()
        {
            return new Artifact("Artifact: " + _nextArtifact++);
        }
    }
}
