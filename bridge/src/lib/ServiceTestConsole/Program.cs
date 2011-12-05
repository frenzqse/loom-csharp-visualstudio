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

            // Methods of AuditDomain can be called by the OpenEngSb
            // after calling RegisterDomainService()
            string destination = "tcp://localhost:6549";
            string domainType = "signal";

            IDomainFactory factory = DomainFactoryProvider.GetDomainFactoryInstance();
            IExampleDomain auditDomain = new ExampleConnector();

            string serviceId = factory.RegisterDomainService(destination, auditDomain, domainType);

            // Services of the OpenEngSb can be called
            // after calling RetrieveDomainProxy()
            // You have to create an AuditDomain with the serviceId: audit-service-1234 on the OpenEngSb
            // Any services can be obtained, as long as the interace is available

            IExampleDomain domain = factory.RetrieveDomainProxy<IExampleDomain>(destination, domainType+"+external-connector-proxy+" + serviceId);

            //domain.DoSomething("test");

            Console.ReadLine();

            factory.UnregisterDomainService(auditDomain);
        }
    }
}
