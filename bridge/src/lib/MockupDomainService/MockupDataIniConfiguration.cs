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
using Innovatian.Configuration;

namespace Org.OpenEngSB.DotNet.Lib.MockupDomainService
{
    class MockupDataIniConfiguration
    {
        private readonly IConfigurationSource _configurationSource;

        public MockupDataIniConfiguration(string configFile)
        {
            _configurationSource = IniConfigurationSource.FromFile(configFile);
        }

        public string GetFileName(Type targetType, string methodName)
        {
            IConfigurationSection section = GetSectionForType(targetType);
            var fileName = GetValue<string>(section, methodName);

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(string.Format("The value in section {0} with the key {1} is empty.", section.Name,
                                                          methodName));
            return fileName;
        }

        private T GetValue<T>(IConfigurationSection section, string method)
        {
            T value;

            bool sectionContainsMethod = section.TryGet(method, out value);
            if (!sectionContainsMethod)
                value = section.Get<T>("*");

            return value;
        }

        private IConfigurationSection GetSectionForType(Type t)
        {
            return GetSectionByName(t.FullName);
        }

        private IConfigurationSection GetSectionByName(string sectionName)
        {
            if (!_configurationSource.Sections.ContainsKey(sectionName))
            {
                return _configurationSource.Sections["*"];
            }

            return _configurationSource.Sections[sectionName];
        }
    }
}
