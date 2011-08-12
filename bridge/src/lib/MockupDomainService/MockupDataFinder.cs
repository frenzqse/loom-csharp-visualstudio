/***
 * Licensed to the Austrian Association for Software Tool Integration (AASTI)
 * under one or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information regarding copyright
 * ownership. The AASTI licenses this file to you under the Apache License,
 * Version 2.0 (the "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 ***/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Innovatian.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Reflection;

namespace Org.OpenEngSB.DotNet.Lib.MockupDomainService
{
    public class MockupDataFinder
    {
        private readonly MockupDataIniConfiguration _configuration;
        private readonly string _basePath;

        public MockupDataFinder(string configFile)
        {
            _basePath = Path.GetDirectoryName(configFile);

            _configuration = new MockupDataIniConfiguration(configFile);
        }

        public string GetMockupData(Type targetType, MethodInfo method, object[] arguments)
        {
            string fileName = GetFile(targetType, method.Name);
            string lowerFileName = fileName.ToLower();
            string ret;

            if (lowerFileName.EndsWith(".xml"))
                ret = GetValueFromXml(fileName, targetType, method, arguments);
            else if (lowerFileName.EndsWith(".bat"))
                ret = GetValueFromBat(fileName, targetType, method.Name, arguments);
            else
                throw new ArgumentException(string.Format("The file {0} is of wrong type, only xml and bat is supported.", fileName));

            ret = ret.Trim();

            if (ret == "#NULL#")
                return null;
            else
                return ret;
        }

        private string GetValueFromXml(string fileName, Type targetType, MethodInfo method, object[] arguments)
        {
            XmlDocument doc = new XmlDocument();
            
            doc.Load(fileName);
            XmlNode methodNode = doc.DocumentElement.SelectSingleNode(string.Format("/root/{0}/{1}", targetType.FullName, method.Name));

            if (methodNode == null)
                throw new ArgumentException(string.Format("The node /root/{0}/{1} is missing in file {2}.", targetType.FullName, method.Name, fileName));

            ParameterInfo[] parameters = method.GetParameters();
            int attributesNull, j;

            for (int i = 0; i <= arguments.Length; i++)
            {
                foreach (XmlNode node in methodNode.SelectNodes("add"))
                {
                    attributesNull = 0;

                    for (j = 0; j < parameters.Length; j++)
                    {
                        XmlAttribute att = node.Attributes[parameters[j].Name];

                        if (att == null)
                        {
                            attributesNull++;

                            if (attributesNull > i)
                                break;
                        }
                        else
                        {
                            if (!(arguments[j] == null && att.InnerText == "#NULL#" ||
                                  arguments[j] != null && att.InnerText == arguments[j].ToString()))
                                break;
                        }
                    }

                    if (j == parameters.Length)
                        return node.InnerText;
                }
            }

            throw new ArgumentException(string.Format("No matching add-node could be found in /root/{0}/{1} in file {2}.", targetType.FullName, method.Name, fileName));
        }

        private string GetValueFromBat(string fileName, Type targetType, string methodName, IEnumerable<object> arguments)
        {
            StringBuilder argumentsBuilder = new StringBuilder(string.Format("{0} {1}", targetType.FullName, methodName));

            foreach (var arg in arguments)
            {
                argumentsBuilder.Append(" ");
                
                if (arg == null)
                    argumentsBuilder.Append("#NULL#");
                else
                    argumentsBuilder.Append(arg.ToString());
            }

            ProcessStartInfo psi = new ProcessStartInfo(fileName, argumentsBuilder.ToString())
                                       {UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = true};

            Process p = new Process {StartInfo = psi};
            p.Start();

            return p.StandardOutput.ReadToEnd();
        }

        private string GetFile(Type targetType, string methodName)
        {
            var fileName = _configuration.GetFileName(targetType, methodName);
            var fullPath = Path.Combine(_basePath, fileName);

            if (!File.Exists(fullPath))
                throw new ArgumentException(string.Format("The file {0} doesn't exist.", fullPath));

            return fullPath;
        }
    }
}
