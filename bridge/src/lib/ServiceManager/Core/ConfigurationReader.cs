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
using System.Linq;
using System.Text;
using System.IO;

namespace ServiceManager.Core
{
    public class ConfigurationReader
    {
        private static string CONFIG_DOMAIN_TYPE = "domainType";
        private static string CONFIG_DESTINATION = "destination";
        private static string CONFIG_ASSEMBLY_PATH = "assemblyPath";
        private static string CONFIG_CLASS_NAME = "className";

        private static char CONFIG_SEPARATOR = '=';

        public string FilePath { get; set; }

        public ConfigurationReader(string filePath)
        {
            FilePath = filePath;
        }

        public ServiceConfiguration LoadConfiguration()
        {
            IDictionary<string, string> config = new Dictionary<string, string>();
            StreamReader reader = File.OpenText(FilePath);
            string line = "";
            string[] tokens = null;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                tokens = line.Split(CONFIG_SEPARATOR);
                
                if(tokens.Length != 2)
                    throw new ApplicationException("invalid line in configuration file: " + FilePath);


                config[tokens[0].Trim()] = tokens[1].Trim();
            }

            if (config.Count != 4 || !CheckConfiguration(config))
                throw new ApplicationException("invalid configuration file: " + FilePath);

            return new ServiceConfiguration(config[CONFIG_DOMAIN_TYPE],
                config[CONFIG_ASSEMBLY_PATH],
                config[CONFIG_DESTINATION],
                config[CONFIG_CLASS_NAME]);
        }

        private bool CheckConfiguration(IDictionary<string, string> configuration)
        {
            if (!configuration.ContainsKey(CONFIG_ASSEMBLY_PATH) ||
                !configuration.ContainsKey(CONFIG_CLASS_NAME) ||
                !configuration.ContainsKey(CONFIG_DESTINATION) ||
                !configuration.ContainsKey(CONFIG_DOMAIN_TYPE))
                return false;

            if (!File.Exists(configuration[CONFIG_ASSEMBLY_PATH]))
                return false;

            return true;
        }
    }
}
