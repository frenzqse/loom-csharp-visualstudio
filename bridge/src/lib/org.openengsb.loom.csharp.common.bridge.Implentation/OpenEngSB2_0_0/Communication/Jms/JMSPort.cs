using System;
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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Apache.NMS;

namespace Org.OpenEngSB.Loom.Csharp.Common.Bridge.Impl.OpenEngSB2_0_0.Communication.Jms
{
    public abstract class JmsPort
    {
        /// <summary>
        /// ActiveMQ NMS
        /// </summary>
        protected IConnection _connection;
        protected IConnectionFactory _factory;
        protected ISession _session;
        protected IDestination _destination;

        protected JmsPort(string destination)
        {
            _connection = null;
            _factory = null;
            _session = null;
            _destination = null;
            Configure(destination);
        }

        private void Configure(string destination)
        {
            Destination dest = new Destination(destination);

            Uri connectionUri = new Uri(dest.Host);
            _factory = new NMSConnectionFactory(connectionUri);
            _connection = _factory.CreateConnection();
            _session = _connection.CreateSession();
            _connection.Start();
            _destination = _session.GetDestination(dest.Queue);
        }

        protected void Close()
        {
            _connection.Close();
        }
    }
}
