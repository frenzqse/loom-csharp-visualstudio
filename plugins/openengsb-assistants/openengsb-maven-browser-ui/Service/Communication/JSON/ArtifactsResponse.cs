using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service.Communication.JSON
{
    class ArtifactsResponse
    {
        public int numFound;
        public int start;
        public ArtifactsDoc[] docs;
    }
}
