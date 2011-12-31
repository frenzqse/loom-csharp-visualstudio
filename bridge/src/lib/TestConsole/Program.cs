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
using org.openengsb.domain.example;
using Org.OpenEngSB.DotNet.Lib.MockupDomainService;
using System.Threading;
using System.ComponentModel;

namespace TestConsole
{
    class Program : IExampleDomain
    {
        [STAThread]
        static void Main(string[] args)
       {
            IExampleDomain iex1 = DomainFactory.Instance.getEventhandler<IExampleDomain>("");
            
            Console.WriteLine(iex1.DoSomething("Hello, i'm here!"));

            IExampleDomain iexService1 = new Program();
            DomainFactory.Instance.RegisterDomainService<IExampleDomain>("", iexService1, "", typeof(Program));

            Console.ReadLine();

            DomainFactory.Instance.Dispose();
        }

        public string DoSomething(ExampleDomain1ExampleEnum arg0)
        {
            return "Argument retrieved: " + arg0.ToString();
        }

        public string DoSomething(string arg0)
        {
            return "Argument retrieved: " + arg0.ToString();
        }

        public string DoSomethingWithLogEvent(org.openengsb.domain.example._event.LogEvent arg0)
        {
            return "Retrieved log-event (" + arg0.Level + " - " + arg0.Message + ")";
        }
    }
}
