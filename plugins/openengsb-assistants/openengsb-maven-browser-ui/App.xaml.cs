using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Wizard wizard = new Wizard(new WizardConfiguration());

            wizard.Steps.Add(new BrowserWindow(wizard));
            wizard.Steps.Add(new DownloadWindow(wizard));

            wizard.DoSteps();

            this.Shutdown();
        }
    }
}
