using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    public class Wizard
    {
        public IList<IWizardStep> Steps { get; set; }

        public WizardConfiguration Configuration { get; set; }

        private FileService _fileService;

        public int Progress { get; private set; }

        public Wizard(WizardConfiguration config)
        {
            Configuration = config;
            _fileService = new FileService();
            _fileService.ProgressEvent += new FileService.UpdateProgressHandler(UpdateProgress);
            Steps = new List<IWizardStep>();
            Progress = 0;
        }

        public void DoSteps()
        {
            IEnumerator<IWizardStep> _stepEnumerator = Steps.GetEnumerator();

            while (_stepEnumerator.MoveNext())
            {
                if (!_stepEnumerator.Current.DoStep())
                    break;
            }
        }

        public void UpdateProgress(int i)
        {
            Progress += i;
        }

        public void DownloadItems()
        {
            IList<string> uris = new List<string>();
            IList<string> names = new List<string>();

            foreach (Item i in Configuration.Items)
            {
                uris.Add(i.Url);
                i.Path = _fileService.CreatePath(Configuration.Path, i.Name);
                names.Add(i.Path);
            }

            _fileService.LoadFilesFrom(uris.ToArray(), names.ToArray());
        }

        public bool DonwloadsComplete()
        {
            return Progress >= Configuration.Items.Count;
        }

        public void CancelDownloads()
        {
            _fileService.CancelDownloads();
        }

        public void CreateSolution()
        {
        }
    }
}
