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
        public IList<string> ProjectReferences { get; set; }
        public string SolutionName { get; set; }
        public string ProjectName { get; set; }

        public WizardConfiguration()
        {
            Path = ".";
            Items = new List<Item>();
            ProjectReferences = new List<string>();
        }
    }
}
