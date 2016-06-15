using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    public interface IWizardStep
    {
        void DoStep();
        void SetNextStep(IWizardStep step);
    }
}
