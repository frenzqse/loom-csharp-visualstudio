using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    class UIArtifact
    {
        public Artifact ArtifactModel { get; set; }
        public IList<UIItemVersion> Versions { get; set; }

        public UIArtifact(Artifact artifact)
        {
            ArtifactModel = artifact;
            Versions = new List<UIItemVersion>();
        }
    }
}
