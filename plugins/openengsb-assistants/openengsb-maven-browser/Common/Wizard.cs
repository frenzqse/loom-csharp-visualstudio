using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service;
using EnvDTE80;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    public class Wizard
    {
        public IWizardStep StartStep { get; set; }

        public WizardConfiguration Configuration { get; set; }

        private FileService _fileService;
        private MavenService _mavenService;
        private DTE2 _visualStudio;

        private int _progress;

        public delegate void ProgressHandler (double progress);
        public event ProgressHandler ProgressChanged;

        public Wizard(DTE2 visualStudio, WizardConfiguration config)
        {
            Configuration = config;
            _mavenService = new MavenService();
            _fileService = new FileService();
            _fileService.ProgressEvent += new FileService.UpdateProgressHandler(UpdateProgress);
            StartStep = null;
            _progress = 0;
            _visualStudio = visualStudio;
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
            generateDlls();
            createSolutionTemplate();
        }

        private void generateDlls()
        {
            ProcessStartInfo srcStartInfo = new ProcessStartInfo(Configuration.WsdlCompilerPath);
            ProcessStartInfo dllStartInfo = new ProcessStartInfo(Configuration.CsharpCompilerPath);
            srcStartInfo.CreateNoWindow = true;
            srcStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            dllStartInfo.CreateNoWindow = true;
            dllStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            foreach (Item i in Configuration.Items)
            {
                string parent = Path.GetDirectoryName(i.Path);
                string baseFileName = Path.GetFileNameWithoutExtension(i.Path);
                string srcFile = baseFileName + ".cs";
                string dllFile = baseFileName + ".dll";
                string srcFilePath = Path.Combine(parent, srcFile);
                string dllFilePath = Path.Combine(parent, dllFile);

                string args = "/noConfig /o:" + srcFilePath + " " + i.Path;
                srcStartInfo.Arguments = args;
                Process proc = Process.Start(srcStartInfo);
                proc.WaitForExit();

                args = "/target:library /out:" + dllFilePath + " " + srcFilePath;
                dllStartInfo.Arguments = args;
                proc = Process.Start(dllStartInfo);
                proc.WaitForExit();

                i.DllPath = dllFilePath;
            }
        }

        private void createSolutionTemplate()
        {
            if (_visualStudio == null)
                return;
            Solution2 solution = (Solution2) _visualStudio.Solution;
            string csTemplatePath = solution.GetProjectTemplate("ConsoleApplication.zip", "CSharp");
            string prjPath = Path.Combine(Configuration.SolutionDirectory, Configuration.ProjectName);
            solution.Create(Configuration.SolutionDirectory, Configuration.SolutionName);
            solution.AddFromTemplate(csTemplatePath, prjPath, Configuration.ProjectName, false);
        }
    }
}
