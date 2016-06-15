using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Threading;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service
{
    public class FileService
    {
        private WebClient _webClient;

        public delegate void UpdateProgressHandler(int i);
        public event UpdateProgressHandler ProgressEvent;

        private IEnumerator<string> _urls;
        private IEnumerator<string> _destinations;

        public FileService()
        {
            _webClient = new WebClient();
            _webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
            _urls = null;
            _destinations = null;
        }

        public void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
        {
            ProgressEvent(1);
            downloadNext();
        }

        private void downloadNext()
        {
            if (_urls == null)
                return;

            if (_destinations == null)
                return;

            if (_urls.MoveNext() && _destinations.MoveNext())
            {
                _webClient.DownloadFileAsync(new Uri(_urls.Current), _destinations.Current);
            }
        }

        public void LoadFileFrom(string url, string destination)
        {
            _webClient.DownloadFile(url, destination);
        }

        public void LoadFilesFrom(string[] urls, string[] destinations)
        {
            if (urls.Length != destinations.Length)
                throw new ArgumentException("Number of urls and destinations doesn't match!");

            _urls = urls.AsEnumerable().GetEnumerator();
            _destinations = destinations.AsEnumerable().GetEnumerator();
            downloadNext();
        }

        public void CancelDownloads()
        {
            _webClient.CancelAsync();
        }

        public string CreatePath(string directory, string fileName)
        {
            return Path.Combine(directory, fileName);
        }
    }
}
