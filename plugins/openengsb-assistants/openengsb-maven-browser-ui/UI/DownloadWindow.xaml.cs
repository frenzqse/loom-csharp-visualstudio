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
        private System.Timers.Timer _timer;
        private bool _canceled;

        public DownloadWindow(Wizard wizard)
        {
            InitializeComponent();
            _wizard = wizard;
            _timer = new System.Timers.Timer();
            _timer.Interval = 100;
            _timer.Elapsed += new ElapsedEventHandler(UpdateProgress);
            _canceled = false;
        }

        private void StartDownload()
        {
            progressBar.Maximum = _wizard.Configuration.Items.Count;
            _timer.Start();
            _wizard.DownloadItems();
        }

        public void UpdateProgress(object sender, EventArgs e)
        {
            //progressBar.Value = _wizard.Progress;
            if (_wizard.DonwloadsComplete())
                _timer.Stop();
        }

        public bool DoStep()
        {
            Show();

            StartDownload();

            Close();

            return !_canceled;
        }

        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            _canceled = true;
            _wizard.CancelDownloads();
            Close();
        }
    }
}