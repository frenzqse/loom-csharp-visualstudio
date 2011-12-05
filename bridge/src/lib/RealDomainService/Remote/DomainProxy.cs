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
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Jms;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Json;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// This class generates generic proxies. All method calls will be forwared to the configured server.
    /// </summary>
    /// <typeparam name="T">Type to proxy.</typeparam>
    public class DomainProxy<T> : RealProxy
    {
        /// <summary>
        /// Name of the queue the server listens to for calls.
        /// </summary>
        private const string HOST_QUEUE = "receive";

        /// <summary>
        /// Id identifying the service instance on the bus.
        /// </summary>
        private string _serviceId;

        /// <summary>
        /// Host string of the server.
        /// </summary>
        private string _host;

        private IMarshaller _marshaller;

        public DomainProxy(string host, string serviceId)
            : base(typeof(T))
        {
            _serviceId = serviceId;
            _host = host; ;
            _marshaller = new JsonMarshaller();
        }

        /// <summary>
        /// Will be invoked when a call to the proxy has been made.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMethodCallMessage callMessage = msg as IMethodCallMessage;

            MethodCallRequest methodCallRequest = ToMethodCallRequest(callMessage);

            string methodCallMsg = _marshaller.MarshallObject(methodCallRequest);
            
            IOutgoingPort portOut = new JmsOutgoingPort(Destination.CreateDestinationString(_host, HOST_QUEUE));
            portOut.Send(methodCallMsg);

            IIncomingPort portIn = new JmsIncomingPort(Destination.CreateDestinationString(_host, methodCallRequest.message.callId));
            string methodReturnMsg = portIn.Receive();

            MethodResult methodReturn = _marshaller.UnmarshallObject(methodCallMsg, typeof(MethodResult)) as MethodResult;

            return ToMessage(methodReturn, callMessage);
        }

        /// <summary>
        /// Builds an IMessage using MethodReturn.
        /// </summary>
        /// <param name="methodReturn">Servers return message</param>
        /// <param name="callMessage"></param>
        /// <returns></returns>
        private IMessage ToMessage(MethodResult methodReturn, IMethodCallMessage callMessage)
        {
            IMethodReturnMessage returnMessage = null;
            switch (methodReturn.type)
            {
                case MethodResult.ReturnType.Exception:
                    returnMessage = new ReturnMessage((Exception)methodReturn.arg, callMessage);
                    break;
                case MethodResult.ReturnType.Void:
                case MethodResult.ReturnType.Object:
                    returnMessage = new ReturnMessage(methodReturn.arg, null, 0, null, callMessage);
                    break;
                default:
                    return null;
            }
            return returnMessage;
        }

        /// <summary>
        /// Builds an MethodCall using IMethodCallMessage.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private MethodCallRequest ToMethodCallRequest(IMethodCallMessage msg)
        {
            Guid id = Guid.NewGuid();

            string methodName = char.ToLower(msg.MethodName[0]) + msg.MethodName.Substring(1);

            Dictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add("serviceId", _serviceId);

            // arbitrary string, maybe not necessary
            metaData.Add("contextId", msg.LogicalCallContext.ToString());

            List<string> classes = new List<string>();
            foreach (object arg in msg.Args)
            {
                LocalType type = new LocalType(arg.GetType());
                classes.Add(type.RemoteTypeFullName);
            }

            MethodCall call = MethodCall.CreateInstance(methodName, msg.Args, metaData, classes);
            String classname = "org.openengsb.core.api.security.model.UsernamePasswordAuthenticationInfo";
            Data data = Data.CreateInstance("admin", "password");
            Authentification authentification = Authentification.createInstance(classname, data, BinaryData.CreateInstance());
            Message message = Message.createInstance(call, id.ToString(), true, "");
            return MethodCallRequest.CreateInstance(authentification,message);
        }

        public new T GetTransparentProxy()
        {
            return (T)base.GetTransparentProxy();
        }
    }
}
