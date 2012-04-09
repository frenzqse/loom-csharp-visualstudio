using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web.Script.Serialization;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Common;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service.Communication.JSON;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service.Communication
{
    class MavenCentralClient
    {
        private WebClient _client;
        private string _baseUrl;
        private string _fileBaseUrl;
        private JavaScriptSerializer _serializer;

        public MavenCentralClient()
        {
            _client = new WebClient();
            _baseUrl = "http://search.maven.org/solrsearch/select?q=";
            _fileBaseUrl = "http://search.maven.org/remotecontent?filepath=";
            _serializer = new JavaScriptSerializer();
        }

        private string getArtifactsUrl(string groupId)
        {
            return _baseUrl + getArtifactsQuery(groupId) + "&wt=json";
        }

        private string getVersionsUrl(string groupId, string artifactId)
        {
            return _baseUrl + getVersionsQuery(groupId, artifactId) + "&wt=json&core=gav";
        }

        private string getArtifactsQuery(string groupId)
        {
            return "g:" + "\"" + groupId + "\"";
        }

        private string getVersionsQuery(string groupId, string artifactId)
        {
            return getArtifactsQuery(groupId) + "+" + "AND" + "+" + "a:" + "\"" + artifactId + "\"";
        }

        public IList<Artifact> GetArtifacts(string groupId)
        {
            string url = getArtifactsUrl(groupId);
            string artifactsString = _client.DownloadString(url);
            ArtifactsListing al = _serializer.Deserialize<ArtifactsListing>(artifactsString);

            IList<Artifact> artifacts = new List<Artifact>();
            foreach (ArtifactsDoc doc in al.response.docs)
            {
                Artifact artifact = new Artifact(groupId, doc.a);
                IList<ItemVersion> versions = GetVersions(groupId, artifact);
                artifact.Versions = versions;
                if(versions.Count > 0)
                    artifacts.Add(artifact);
            }
            return artifacts;
        }

        public IList<ItemVersion> GetVersions(string groupId, Artifact artifact)
        {
            string url = getVersionsUrl(groupId, artifact.Id);
            string versionsString = _client.DownloadString(url);
            VersionsListing vl = _serializer.Deserialize<VersionsListing>(versionsString);
            IList<ItemVersion> versions = new List<ItemVersion>();

            foreach (VersionsDoc doc in vl.response.docs)
            {
                ItemVersion iv = new ItemVersion(doc.v, artifact);
                iv.Items = getFilteredItems(iv, doc.ec);
                if (iv.Items.Count > 0)
                    versions.Add(iv);
            }
            return versions;
        }

        private IList<Item> getFilteredItems(ItemVersion iv, string[] files)
        {
            IList<Item> items = new List<Item>();
            foreach (string file in files)
            {
                if (file.EndsWith("DomainEvents.wsdl") || file.EndsWith("Domain.wsdl"))
                {
                    String url = generateFileUrl(file, iv);
                    Item i = new Item(generateFileName(file, iv),url, iv);
                    items.Add(i);
                }
            }
            return items;
        }

        private string generateFileUrl(string fileName, ItemVersion version)
        {
            string parameter = version.ParentArtifact.GroupId.Replace(".", "/");
            parameter += "/" + version.ParentArtifact.Id;
            parameter += "/" + version.Id;
            parameter += "/" + generateFileName(fileName, version);
            return _fileBaseUrl + parameter;
        }

        private string generateFileName(string fileName, ItemVersion version)
        {
            return version.ParentArtifact.Id + "-" + version.Id + fileName;
        }

        public string GetItemDownloadUrl(string groupdId, string artifactId, string itemName)
        {
            return groupdId + artifactId + itemName;
        }
    }
}
