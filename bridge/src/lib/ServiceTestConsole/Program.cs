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
using System.Reflection;
using System.IO;

namespace ServiceTestConsole
{
    class Program
    {
        /// <summary>
        /// This verion works with the openEngS 2.3.0-Snapshot Framwork
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            log4net.Config.BasicConfigurator.Configure();

            string destination = "tcp://localhost:6549";
            string domainName = "signal";

            IDomainFactory factory = DomainFactoryProvider.GetDomainFactoryInstance();
  
            ISignalDomainSoapBinding localDomain = new SignalConnector();

            //Register the connecter on the osenEngSB
            factory.RegisterDomainService(destination, localDomain, domainName, typeof(ISignalDomainEventsSoapBinding));
            //Get a remote handler, to raise events on obenEngSB
            ISignalDomainEventsSoapBinding remotedomain = factory.getEventhandler<ISignalDomainEventsSoapBinding>(destination);
            updateMeEvent events=new updateMeEvent();
            events.name = "updateMe";
            events.lastKnownVersion = "1321503714918";
            events.query="cpuNumber:1";
            events.origin = factory.getDomainTypServiceId();
            remotedomain.raiseUpdateMeEvent(events);
            Console.ReadKey();
            factory.UnregisterDomainService(localDomain);
            
        }
    }
}
