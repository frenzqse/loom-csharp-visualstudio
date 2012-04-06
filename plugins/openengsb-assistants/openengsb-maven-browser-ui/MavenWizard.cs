using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants
{
    public class MavenWizard
    {
        private Wizard _wizard;

        public MavenWizard()
        {
            _wizard = new Wizard(new WizardConfiguration());
            _wizard.Steps.Add(new BrowserWindow(_wizard));
            _wizard.Steps.Add(new DownloadWindow(_wizard));
            _wizard.Steps.Add(new CreateProjectWindow(_wizard));
        }

        public void DoSteps()
        {
            _wizard.DoSteps();
        }
    }
}
