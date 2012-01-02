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
using Apache.NMS;
namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Jms
{
    public class JmsIncomingPort : JmsPort, IIncomingPort
    {
        IMessageConsumer _consumer;

        public JmsIncomingPort(string destination) : base(destination)
        {
            _consumer = _session.CreateConsumer(_destination);
        }

        /// <summary>
        /// Waits for a message on the preconfigured queue.
        /// Blocks until a message in received or the connection is closed.
        /// </summary>
        /// <returns>Read message. Null if the connection is closed.</returns>
        public string Receive()
        {
            ITextMessage message = _consumer.Receive() as ITextMessage;

            if (message == null)
                return null;
            return message.Text;
        }

        public new void Close()
        {
            base.Close();
            _consumer.Close();
        }
    }
}
