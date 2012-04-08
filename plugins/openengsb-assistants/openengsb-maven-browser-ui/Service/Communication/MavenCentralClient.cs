using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service.Communication.JSON;

namespace Org.OpenEngSB.Loom.Csharp.VisualStudio.Plugins.Assistants.Service.Communication
{
    class MavenCentralClient
    {
        private WebClient _client;
        private string _baseUrl;

        public MavenCentralClient()
        {
            _client = new WebClient();
            _baseUrl = "http://search.maven.org/solrsearch/select?";
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

        public ArtifactsListing GetArtifacts(string groupId)
        {
            return new ArtifactsListing();
        }

        public VersionsListing GetVersions(string groupId, string artifactId)
        {
            return new VersionsListing();
        }

        public string GetItemDownloadUrl(string groupdId, string artifactId, string itemName)
        {
            return groupdId + artifactId + itemName;
        }
    }
}
