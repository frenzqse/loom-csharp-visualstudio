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
using System.Threading;
using System.Reflection;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Jms;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Communication.Json;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Common;
using System.IO;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// This class builds reverse proxies for resources (class instances) on the
    /// client side for the bus.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DomainReverseProxy<T>: IStoppable
    {
        private const string _CREATION_QUEUE = "receive";
        private const string _CREATION_SERVICE_ID = "connectorManager";
        private const string _CREATION_METHOD_NAME = "create";
        private const string _CREATION_DELETE_METHOD_NAME = "delete";
        private const string _CREATION_PORT = "jms-json";
        private const string _CREATION_CONNECTOR_TYPE = "external-connector-proxy";

        // Thread listening for messages
        private Thread _queueThread;

        // IO port
        private IIncomingPort _portIn;

        private string _destination;

        /// <summary>
        /// ServiceId of the proxy on the bus
        /// </summary>
        private string _serviceId;

        /// <summary>
        ///  DomainType string required for OpenengSb
        /// </summary>
        private string _domainType;

        /// <summary>
        /// domain-instance to act as reverse-proxy for
        /// </summary>
        private T _domainService;
        public T DomainService 
        {
            get { return _domainService; }
        }

        /// <summary>
        /// flag indicating if the listening thread should run
        /// </summary>

        private bool _isEnabled;

        private IMarshaller _marshaller;

        /// <summary>
        /// Identifies the service-instance.
        /// </summary>
        private ConnectorId _connectorId;

        public DomainReverseProxy(T domainService, string host, string serviceId, string domainType)
        {
            _marshaller = new JsonMarshaller();
            _isEnabled = true;
            _domainService = domainService;
            _destination = Destination.CreateDestinationString(host, serviceId);
            _queueThread = null;
            _serviceId = serviceId;
            _domainType = domainType;
            _portIn = new JmsIncomingPort(_destination);
            _connectorId = null;
        }

        /// <summary>
        /// Starts a thread which waits for messages.
        /// An exception will be thrown, if the method has already been called.
        /// </summary>
        public void Start()
        {
            if (_queueThread != null)
                throw new ApplicationException("QueueThread already started!");

            _isEnabled = true;
            CreateRemoteProxy();
            // start thread which waits for messages
            _queueThread = new Thread(
                new ThreadStart(Listen)
                );

            _queueThread.Start();
        }

        /// <summary>
        /// Creates an Proxy on the bus.
        /// </summary>
        private void CreateRemoteProxy()
        {
            IDictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add("serviceId", _CREATION_SERVICE_ID);
            Guid id = Guid.NewGuid();

            String classname = "org.openengsb.core.api.security.model.UsernamePasswordAuthenticationInfo";
            Data data = Data.CreateInstance("admin", "password");
            Authentification authentification = Authentification.createInstance(classname, data, BinaryData.CreateInstance());

            IList<string> classes = new List<string>();
            classes.Add("org.openengsb.core.api.model.ConnectorId");
            classes.Add("org.openengsb.core.api.model.ConnectorDescription");
            Destination tmp = new Destination(_destination);
            tmp.Queue = id.ToString();
            //_destination = tmp.FullDestination;
            IList<object> args = new List<object>();
            ConnectorDescription connectorDescription = new ConnectorDescription();
            connectorDescription.attributes.Add("serviceId", _serviceId);
            connectorDescription.attributes.Add("portId", _CREATION_PORT);
            connectorDescription.attributes.Add("destination", _destination);

            _connectorId = new ConnectorId();
            _connectorId.connectorType = _CREATION_CONNECTOR_TYPE;
            _connectorId.instanceId = _serviceId;
            _connectorId.domainType = _domainType;

            args.Add(_connectorId);
            args.Add(connectorDescription);

            MethodCall creationCall = MethodCall.CreateInstance(_CREATION_METHOD_NAME, args, metaData, classes);


            Message message = Message.createInstance(creationCall, id.ToString(), true, "");
            MethodCallRequest callRequest = MethodCallRequest.CreateInstance(authentification, message);
            callRequest.message.methodCall = creationCall;

            Destination destination = new Destination(_destination);
            destination.Queue = _CREATION_QUEUE;

            IOutgoingPort portOut = new JmsOutgoingPort(destination.FullDestination);
            string request = _marshaller.MarshallObject(callRequest);
            portOut.Send(request);            
        }

        /// <summary>
        /// Deletes the created remote proxy on the bus.
        /// </summary>
        private void DeleteRemoteProxy()
        {
            IDictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add("serviceId", _CREATION_SERVICE_ID);

            IList<string> classes = new List<string>();
            classes.Add("org.openengsb.core.api.model.ConnectorId");

            IList<object> args = new List<object>();
            args.Add(_connectorId);

            MethodCall deletionCall = MethodCall.CreateInstance(_CREATION_DELETE_METHOD_NAME, args, metaData, classes);

            Guid id = Guid.NewGuid();
            String classname = "org.openengsb.core.api.security.model.UsernamePasswordAuthenticationInfo";
            Data data = Data.CreateInstance("admin", "password");
            Authentification authentification = Authentification.createInstance(classname, data, BinaryData.CreateInstance());

            Message message = Message.createInstance(deletionCall, id.ToString(), true, "");
            MethodCallRequest callRequest = MethodCallRequest.CreateInstance(authentification,message);

            Destination destination = new Destination(_destination);
            destination.Queue = _CREATION_QUEUE;

            IOutgoingPort portOut = new JmsOutgoingPort(destination.FullDestination);
            string request = _marshaller.MarshallObject(callRequest);
            portOut.Send(request);

            IIncomingPort portIn = new JmsIncomingPort(Destination.CreateDestinationString(destination.Host, callRequest.message.callId));
            string reply = portIn.Receive();

            MethodResultMessage result = _marshaller.UnmarshallObject(reply, typeof(MethodResultMessage)) as MethodResultMessage;
            if (result.message.result.type == MethodResult.ReturnType.Exception)
                throw new ApplicationException("Remote Exception while deleting service proxy");
        }

        /// <summary>
        /// Blocks an waits for messages.
        /// </summary>
        private void Listen()
        {
            while (_isEnabled)
            {
                String textMsg = _portIn.Receive();

                if (textMsg == null)
                    continue;
                
                MethodCallRequest methodCallRequest = _marshaller.UnmarshallObject(textMsg, typeof(MethodCallRequest)) as MethodCallRequest;

                MethodResultMessage methodReturnMessage = CallMethod(methodCallRequest);

                if (methodCallRequest.message.answer)
                {
                    string returnMsg = _marshaller.MarshallObject(methodReturnMessage);
                    Destination dest = new Destination(_destination);
                    IOutgoingPort portOut = new JmsOutgoingPort(Destination.CreateDestinationString(dest.Host, methodCallRequest.message.callId));
                    portOut.Send(returnMsg);
                }
            }
        }

        /// <summary>
        /// Calls a method according to MethodCall.
        /// </summary>
        /// <param name="methodCall">Description of the call.</param>
        /// <returns></returns>
        private MethodResultMessage CallMethod(MethodCallRequest request)
        {
            MethodInfo methInfo = FindMethodInDomain(request.message.methodCall);
            if (methInfo == null)
                throw new ApplicationException("No corresponding method found");

            object[] arguments = CreateMethodArguments(request.message.methodCall);

            object returnValue = null;
            try
            {
                returnValue = methInfo.Invoke(_domainService, arguments);
            }
            catch (Exception ex)
            {
                return CreateMethodReturn(MethodResult.ReturnType.Exception, ex, request.message.callId);
            }

            MethodResultMessage returnMsg = null;

            if (returnValue == null)
                returnMsg = CreateMethodReturn(MethodResult.ReturnType.Void, "null", request.message.callId);
            else
                returnMsg = CreateMethodReturn(MethodResult.ReturnType.Object, returnValue, request.message.callId);

            return returnMsg;
        }

        /// <summary>
        /// Builds an return message.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnValue"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        private MethodResultMessage CreateMethodReturn(MethodResult.ReturnType type, object returnValue, string correlationId)
        {
            MethodResult methodResult = new MethodResult();
            methodResult.type = type;
            methodResult.arg = returnValue;
            MethodResultMessage methodResultMessage = new MethodResultMessage();
            methodResultMessage.message=new MessageResult();
            methodResultMessage.message.callId = correlationId;

            if (returnValue == null)
                methodResult.className = "null";
            else
                methodResult.className = new LocalType(returnValue.GetType()).RemoteTypeFullName;

            methodResult.metaData = new Dictionary<string, string>();

            methodResultMessage.message.result = methodResult;
            return methodResultMessage;
        }

        /// <summary>
        /// Unmarshalls the arguments of a MethodCall.
        /// </summary>
        /// <param name="methodCall"></param>
        /// <returns></returns>
        private object[] CreateMethodArguments(MethodCall methodCall)
        {
            IList<object> args = new List<object>();

            // load assembly containing domains
            Assembly asm = Assembly.LoadFile(Path.GetFullPath("openengsbDomains.dll"));

            for (int i = 0; i < methodCall.args.Count; ++i)
            {
                object arg = methodCall.args[i];
                RemoteType remoteType = new RemoteType(methodCall.classes[i]);
                Type type = asm.GetType(remoteType.LocalTypeFullName);
                
                if (type == null)
                    type = Type.GetType(remoteType.LocalTypeFullName);

                if (type == null)
                    throw new ApplicationException("no corresponding local type found");

                object obj = null;
                if (type.IsPrimitive || type.Equals(typeof(string)))
                {
                    obj = arg;
                }
                else if (type.IsEnum)
                {
                    obj = Enum.Parse(type,(string) arg);
                }
                else
                {
                    obj = _marshaller.UnmarshallObject(arg.ToString(), type);
                }
                args.Add(obj);
            }

            return args.ToArray();
        }

        /// <summary>
        /// Tries to find the method that should be called.
        /// </summary>
        /// <param name="methodCallWrapper"></param>
        /// <returns></returns>
        private MethodInfo FindMethodInDomain(MethodCall methodCallWrapper)
        {
            foreach (MethodInfo methodInfo in _domainService.GetType().GetMethods())
            {
                if (methodCallWrapper.methodName.ToLower() != methodInfo.Name.ToLower())
                {
                    continue;
                }

                if (methodInfo.GetParameters().Length != methodCallWrapper.args.Count)
                {
                    continue;
                }
                if (!TypesAreEqual(methodCallWrapper.classes, methodInfo.GetParameters()))
                {
                    continue;
                }

                return methodInfo;
            }

            return null;
        }

        /// <summary>
        /// Tests if the list of type names are equal to the types of the method parameter.
        /// </summary>
        /// <param name="typeStrings"></param>
        /// <param name="parameterInfos"></param>
        /// <returns></returns>
        private bool TypesAreEqual(IList<string> typeStrings, ParameterInfo[] parameterInfos)
        {
            if (typeStrings.Count != parameterInfos.Length)
                throw new ApplicationException("length of type-string-arrays are not equal");

            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                if (!TypeIsEqual(typeStrings[i], parameterInfos[i].ParameterType))
                {
                    return false;
                }
            }

            return true;
        }

        private bool TypeIsEqual(string remoteType, Type localType)
        {
            RemoteType remote_typ = new RemoteType(remoteType);
            // leading underscore fix
            return (remote_typ.LocalTypeFullName == localType.FullName);
        }

        /// <summary>
        /// Stops the queue listening for messages and deletes the proxy on the bus.
        /// </summary>
        public void Stop()
        {
            if (_queueThread != null)
            {
                _isEnabled = false;
                _portIn.Close();
                DeleteRemoteProxy();
            }
        }
    }
}
