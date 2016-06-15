using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI;
using EnvDTE80;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants
{
    public class MavenWizard
    {
        private Wizard _wizard;

        public MavenWizard(DTE2 visualStudio)
        {
            _wizard = new Wizard(visualStudio, new WizardConfiguration());
            IWizardStep step1 = new BrowserWindow(_wizard);
            IWizardStep step2 = new DownloadWindow(_wizard);
            IWizardStep step3 = new CreateProjectWindow(_wizard);
            step1.SetNextStep(step2);
            step2.SetNextStep(step3);

            _wizard.StartStep = step1;
        }

        public void DoSteps()
        {
            _wizard.DoSteps();
        }
    }
}
