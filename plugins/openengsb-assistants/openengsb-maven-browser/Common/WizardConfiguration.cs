using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common
{
    public class WizardConfiguration
    {
        public string CsharpCompilerPath;
        public string WsdlCompilerPath;

        public string Path { get; set; }
        public IList<Item> Items { get; set; }
        public IList<string> ProjectReferences { get; set; }
        public string SolutionName { get; set; }
        public string ProjectName { get; set; }
        public string SolutionDirectory { get; set; }

        public WizardConfiguration()
        {
            Path = ".";
            Items = new List<Item>();
            ProjectReferences = new List<string>();
            CsharpCompilerPath = "";
            WsdlCompilerPath = "";
            SolutionDirectory = "";
            locateCsharpCompiler();
            locateWsdlCompiler();
        }

        private void locateCsharpCompiler()
        {
            string path = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe";
            if (File.Exists(path))
                CsharpCompilerPath = path;

            path = @"C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe";
            if (File.Exists(path))
                CsharpCompilerPath = path;

            if (CsharpCompilerPath == string.Empty)
                throw new ApplicationException("csc.exe not found");
        }

        private void locateWsdlCompiler()
        {
            string path = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\x64\svcutil.exe";
            if (File.Exists(path))
                WsdlCompilerPath = path;

            path = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\x64\svcutil.exe";
            if (File.Exists(path))
                WsdlCompilerPath = path;

            path = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\NETFX 4.0 Tools\svcutil.exe";
            if (File.Exists(path))
                WsdlCompilerPath = path;

            path = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin\svcutil.exe";
            if (File.Exists(path))
                WsdlCompilerPath = path;

            if (WsdlCompilerPath == string.Empty)
                throw new ApplicationException("svcutil.exe not found");
        }
    }
}
