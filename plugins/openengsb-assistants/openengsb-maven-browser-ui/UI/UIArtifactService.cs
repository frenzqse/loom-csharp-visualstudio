using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    class UIArtifactService
    {

        /// <summary>
        /// Mirroring the artifacts tree to a separate tree for ui reprentation.
        /// </summary>
        /// <returns></returns>
        public IList<UIArtifact> LoadUIArtifacts()
        {
            IList<UIArtifact> artifacts = new List<UIArtifact>();
            
            MavenService service = new MavenService();

            IList<Artifact> model_artifacts = service.LoadWsdlArtifacts();

            foreach(Artifact a  in model_artifacts)
            {
                UIArtifact ua = new UIArtifact(a);

                foreach (ItemVersion v in a.Versions)
                {
                    UIItemVersion uv = new UIItemVersion(v);

                    foreach (Item i in v.Items)
                    {
                        UIItem ui = new UIItem(i);
                        uv.Items.Add(ui);
                    }

                    ua.Versions.Add(uv);
                }

                artifacts.Add(ua);
            }

            return artifacts;
        }
    }
}
