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
using org.openengsb.domain.auditing;
using org.openengsb.core.api;
using log4net;

namespace ServiceTestConsole
{
    public class AuditConnector : IAuditingDomain
    {
        ILog _logger = LogManager.GetLogger(typeof(AuditConnector));

        public void Audit(Event ev)
        {
            _logger.Info("an audit: " + ev.name);
        }

        public IList<Event> GetAudits()
        {
            IList<Event> events = new List<Event>();
            events.Add(new Event { name = "audit 1", processId = 1 });
            events.Add(new Event { name = "audit 2", processId = 2 });
            _logger.Info("getting audits: " + events.ToString());
            return events;
        }
    }
}
