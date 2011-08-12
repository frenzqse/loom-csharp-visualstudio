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
    public class ConfigurationDirectory
    {
        public static string CONFIG_FILE_EXTENSION = ".conf";

        private string _path;

        public ConfigurationDirectory(string path)
        {
            CheckDirectory(path);
            _path = path;
        }

        public IList<ServiceConfiguration> LoadConfigurations()
        {
            IList<ServiceConfiguration> configurations = new List<ServiceConfiguration>();
            foreach (string filePath in Directory.GetFiles(_path, "*"+CONFIG_FILE_EXTENSION))
            {
                ConfigurationReader reader = new ConfigurationReader(filePath);
                configurations.Add(reader.LoadConfiguration());
            }
            return configurations;
        }

        private void CheckDirectory(string path)
        {
            if (!Directory.Exists(path))
                throw new ApplicationException("Path doesn't point to a directory: " + path);
        }
    }
}
