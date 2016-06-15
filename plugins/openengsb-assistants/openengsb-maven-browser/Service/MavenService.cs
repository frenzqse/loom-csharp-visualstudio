using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Testing;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service.Communication;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service
{
    public class MavenService
    {
        public const string GROUP_ID = "org.openengsb.domain";

        public string Repository { get; set; }
        private MavenCentralClient _client;

        public MavenService()
        {
            _client = new MavenCentralClient();
        }

        public IList<Artifact> LoadWsdlArtifacts()
        {
            return _client.GetArtifacts(GROUP_ID);
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
