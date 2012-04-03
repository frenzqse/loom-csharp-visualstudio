using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Testing;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service
{
    class MavenService
    {
        public string Repository { get; set; }

        public IList<Artifact> LoadWsdlArtifacts()
        {
            return GenerateDefaultArtifacts();
        }

        private IList<Artifact> GenerateDefaultArtifacts()
        {
            DataGenerator gen = new DataGenerator();
            IList<Artifact> artifacts = new List<Artifact>();

            for (int i = 0; i < 2; i++)
            {
                Artifact a = gen.NextArtifact();
                for (int j = 0; j < 2; j++)
                {
                    ItemVersion v = gen.NextVersion(a);
                    for (int k = 0; k < 2; k++)
                    {
                        Item it = gen.NextItem(v);
                        v.Items.Add(it);
                    }
                    a.Versions.Add(v);
                }
                artifacts.Add(a);
            }

            return artifacts;
        }
    }
}
