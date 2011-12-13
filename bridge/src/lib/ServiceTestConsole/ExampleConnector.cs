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
using log4net;
namespace Org.OpenEngSB.DotNet.Lib.RealDomainService
{
    class ExampleConnector : IExampleDomain
    {
        private ILog _logger = LogManager.GetLogger(typeof(ExampleConnector));

        public string DoSomething(ExampleDomain1ExampleEnum arg0)
        {
            _logger.Info("ExampleDomain.DoSomething(ExampleDomain1ExampleEnum)");
            return "something done";
        }

        public string DoSomething(string arg0)
        {
            _logger.Info("ExampleDomain.DoSomething(string)");
            return "something done with string";
        }

        public string DoSomethingWithLogEvent(org.openengsb.domain.example._event.LogEvent arg0)
        {
            _logger.Info("ExampleDomain.DoSomething(LogEvent)");
            return "something done with logger";
        }
        public void setDomainId(String element)
        {
            _logger.Info("setDomainId:"+element);
        }
        public void setConnectorId(String ASDA)
        {
            _logger.Info("setConnectorId:" + ASDA);
        }
    }
}
