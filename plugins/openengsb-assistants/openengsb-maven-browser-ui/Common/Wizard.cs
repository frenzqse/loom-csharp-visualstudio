using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    public class Wizard
    {
        public IWizardStep StartStep { get; set; }

        public WizardConfiguration Configuration { get; set; }

        private FileService _fileService;
        private MavenService _mavenService;

        private int _progress;

        public delegate void ProgressHandler (double progress);
        public event ProgressHandler ProgressChanged;

        public Wizard(WizardConfiguration config)
        {
            Configuration = config;
            _mavenService = new MavenService();
            _fileService = new FileService();
            _fileService.ProgressEvent += new FileService.UpdateProgressHandler(UpdateProgress);
            StartStep = null;
            _progress = 0;
        }

        public void DoSteps()
        {
            StartStep.DoStep();
        }

        public void UpdateProgress(int i)
        {
            _progress += i;
            ProgressChanged(GetProgress());
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

        public IList<Artifact> LoadArtifacts()
        {
            return _mavenService.LoadWsdlArtifacts();
        }

        public bool DonwloadsComplete()
        {
            return _progress >= Configuration.Items.Count;
        }

        public double GetProgress()
        {
            if (Configuration.Items.Count == 0)
                return 1;

            return ((double)_progress) / Configuration.Items.Count;
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
