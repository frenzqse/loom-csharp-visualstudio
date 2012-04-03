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


        public FileService()
        {
            _webClient = new WebClient();
            _webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCallback);
        }

        public void DownloadFileCallback(object sender, AsyncCompletedEventArgs e)
        {
            ProgressEvent(1);
        }

        public void LoadFileFrom(string url, string destination)
        {
            _webClient.DownloadFile(url, destination);
        }

        public void LoadFilesFrom(string[] urls, string[] destinations)
        {
            if (urls.Length != destinations.Length)
                throw new ArgumentException("Number of urls and destinations doesn't match!");

            for(int i = 0; i < urls.Length; i++)
            {
                //_webClient.DownloadFileAsync(new Uri(urls[i]), destinations[i]);
                Thread.Sleep(500);
                ProgressEvent(1);
            }
        }

        public void CancelDownloads()
        {
            _webClient.CancelAsync();
        }
    }
}
