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
using Org.OpenEngSB.DotNet.Lib.RealDomainService;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Jms;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Json;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote;
using Org.OpenEngSB.DotNet.Lib.DomainService;
using org.openengsb.domain.example;
using org.openengsb.domain.auditing;
using org.openengsb.core.api;
using System.Reflection;
using System.IO;

namespace ServiceTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();
            
            string destination = "tcp://localhost:6549";
            string domainType = "example";

            IDomainFactory factory = DomainFactoryProvider.GetDomainFactoryInstance();
            IExampleDomain exampleConnector = new ExampleConnector();
            // string serviceId = factory.RegisterDomainService("tcp://localhost:6549", exampleConnector, "example");
            string serviceId = factory.RegisterDomainService(destination, exampleConnector, domainType);

            // IExampleDomain domain = factory.RetrieveDomainProxy<IExampleDomain>("tcp://localhost:6549", "example+external-connector-proxy+" + serviceId);
            IExampleDomain domain = factory.RetrieveDomainProxy<IExampleDomain>(destination, domainType+"+external-connector-proxy+" + serviceId);
            String result=domain.DoSomething("Hello World");
            
            Console.WriteLine(result);
            factory.UnregisterDomainService(exampleConnector);
            Console.ReadKey();
        }
    }
}
