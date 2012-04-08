using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service;

namespace maven_central_client
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<Artifact> artifacts = new MavenService().LoadWsdlArtifacts();
            foreach (Artifact a in artifacts)
            {
                Console.Write("Artifact: ");
                Console.WriteLine(a.Id);
                foreach (ItemVersion v in a.Versions)
                {
                    Console.Write("  Version: ");
                    Console.WriteLine(v.Id);

                    foreach (Item i in v.Items)
                    {
                        Console.Write("    Item: ");
                        Console.WriteLine(i.Name);
                        Console.WriteLine("    " + i.Url);
                    }
                }
            }
            Console.ReadKey();
        }
    }
}
