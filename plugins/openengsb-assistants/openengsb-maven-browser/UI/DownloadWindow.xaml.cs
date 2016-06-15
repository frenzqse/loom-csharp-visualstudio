using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.UI
{
    /// <summary>
    /// Interaction logic for DownloadWindow.xaml
    /// </summary>
    public partial class DownloadWindow : Window, IWizardStep
    {
        private Wizard _wizard;
        private IWizardStep _nextStep;

        public DownloadWindow(Wizard wizard)
        {
            InitializeComponent();
            _wizard = wizard;
            _wizard.ProgressChanged += new Wizard.ProgressHandler(UpdateProgress);
            _nextStep = null;
            progressBar.Maximum = 1;
            progressBar.Minimum = 0;
        }

        public void UpdateProgress(double progress)
        {
            Dispatcher.BeginInvoke((Action)delegate()
            {
                progressBar.Value = progress;
                
                if (progress == 1)
                {
                    if (this._nextStep == null)
                        return;

                    Close();

                    _nextStep.DoStep();
                }
            });
        }

        public void DoStep()
        {
            Thread thread = new Thread(new ThreadStart(_wizard.DownloadItems));
            thread.Start();

            ShowDialog();
        }

        public void SetNextStep(IWizardStep step)
        {
            _nextStep = step;
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            _wizard.CancelDownloads();
            Close();
        }
    }
}