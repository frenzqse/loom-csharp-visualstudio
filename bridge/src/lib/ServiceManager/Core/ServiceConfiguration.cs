﻿/***
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
using System.Reflection;
using System.IO;

namespace ServiceManager.Core
{
    public class ServiceConfiguration
    {
        public string DomainType { get; set; }
        public string AssemblyPath { get; set; }
        public string DestinationString { get; set; }
        public string ServiceClassName { get; set; }

        private object _serviceInstance;

        public ServiceConfiguration( string domainType, string assemblyPath,
            string destinationString, string serviceClassName)
        {
            DomainType = domainType;
            AssemblyPath = assemblyPath;
            DestinationString = destinationString;
            ServiceClassName = serviceClassName;
            _serviceInstance = null;
        }

        public object GetServiceInstance()
        {
            if (_serviceInstance != null)
                return _serviceInstance;

            Assembly asm = Assembly.LoadFile(Path.GetFullPath(AssemblyPath));
            Type type = asm.GetType(ServiceClassName);
            _serviceInstance = Activator.CreateInstance(type);
            return _serviceInstance;
        }
    }
}
