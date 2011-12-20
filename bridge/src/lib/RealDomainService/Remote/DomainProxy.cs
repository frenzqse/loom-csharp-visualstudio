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
using System.Web.Services.Protocols;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;
namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// This class generates generic proxies. All method calls will be forwared to the configured server.
    /// </summary>
    /// <typeparam name="T">Type to proxy.</typeparam>
    public class DomainProxy<T> : RealProxy
    {
        #region Variables
        /// <summary>
        /// Name of the queue the server listens to for calls.
        /// </summary>
        private const string HOST_QUEUE = "receive";

        /// <summary>
        /// Id identifying the service instance on the bus.
        /// </summary>
        private string _serviceId;
        /// <summary>
        /// Domain type
        /// </summary>
        private string domainType;

        /// <summary>
        /// Host string of the server.
        /// </summary>
        private string _host;

        private IMarshaller _marshaller;
        #endregion
        #region Constructors
        public DomainProxy(string host, string serviceId,String domainType)
            : base(typeof(T))
        {
            _serviceId = serviceId;
            this.domainType = domainType;
            _host = host; ;
            _marshaller = new JsonMarshaller();
        }
        #endregion
        #region Public Methods
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

            MethodResultMessage methodReturn = _marshaller.UnmarshallObject(methodReturnMsg, typeof(MethodResultMessage)) as MethodResultMessage;

            return ToMessage(methodReturn.message.result, callMessage);
        }
        public new T GetTransparentProxy()
        {
            return (T)base.GetTransparentProxy();
        }
        #endregion
        #region Private methods
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
        /// Takes a Namespace as input, reverse the elements and returns the package structure from java
        /// </summary>
        /// <param name="url">Namespace URL</param>
        /// <returns>Java Package structure</returns>
        private String reverseURL(String url)
        {           
            String tmp=url.Replace("http://","");
            tmp=tmp.Replace("/","");
            String[] elements = tmp.Split('.');
            int i;
            String result="";
            for (i = elements.Length-1; i >= 0; i--)
            {
                if (i != 0) result += elements[i] + ".";
                else result += elements[i];                
            }
            return result;
        }

        /// <summary>
        /// Search in the interface for the Namespace (equal to the package structure in java)
        /// </summary>
        /// <param name="fieldname">Method name or Parameter name</param>
        /// <returns>Packagename</returns>
        private String getPackageName(String fieldname)
        {
            Boolean ismethod = false;
            Type type = typeof(T);
            MethodInfo method = type.GetMethod(fieldname);
            ismethod = method != null;
            if (ismethod)
            {
                SoapDocumentMethodAttribute soapAttribute;
                foreach (Attribute attribute in method.GetCustomAttributes(false))
                {
                    if (attribute is SoapDocumentMethodAttribute)
                    {
                        soapAttribute = attribute as SoapDocumentMethodAttribute;
                        return reverseURL(soapAttribute.RequestNamespace);
                    }
                }
            }
            else
            {
                Assembly ass = typeof(T).Assembly;
                type = ass.GetType(fieldname);
                foreach (Attribute attribute in Attribute.GetCustomAttributes(type))
                {
                    if (attribute is XmlTypeAttribute)
                    {
                        XmlTypeAttribute xmltype = attribute as XmlTypeAttribute;
                        return reverseURL(xmltype.Namespace);
                    }
                }
            }
            return fieldname;
        }
        /// <summary>
        /// Makes the first character to a upper character
        /// </summary>
        /// <param name="element">Element to edit</param>
        /// <returns>String with the first character upper</returns>
        private String firstLetterToUpper(String element)
        {
            if (element.Length <= 1) return element.ToUpper();
            String first = element.Substring(0, 1);
            first = first.ToUpper();
            String tmp = element.Substring(1);
            return first + tmp;
        }
        /// <summary>
        /// Builds an MethodCall using IMethodCallMessage.
        /// TODO if bug ??? is fixed replace classes.Add(getPackageName(type.RemoteTypeFullName) + ".event." + firstLetterToUpper(type.RemoteTypeFullName)); with classes.Add(getPackageName(type.RemoteTypeFullName) + "." + firstLetterToUpper(type.RemoteTypeFullName));
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private MethodCallRequest ToMethodCallRequest(IMethodCallMessage msg)
        {
            Guid id = Guid.NewGuid();

            string methodName = msg.MethodName;
            Dictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add("serviceId", "domain."+domainType+".events");

            // arbitrary string, maybe not necessary
            metaData.Add("contextId", "foo");

            List<string> classes = new List<string>();
            List<string> realClassImplementation = new List<string>();
            foreach (object arg in msg.Args)
            {
                LocalType type = new LocalType(arg.GetType());
                realClassImplementation.Add(getPackageName(type.RemoteTypeFullName));
                classes.Add(getPackageName(type.RemoteTypeFullName) + ".event." + firstLetterToUpper(type.RemoteTypeFullName));
            }

            MethodCall call = MethodCall.CreateInstance(methodName, msg.Args, metaData, classes, realClassImplementation);
            String classname = "org.openengsb.core.api.security.model.UsernamePasswordAuthenticationInfo";
            Data data = Data.CreateInstance("admin", "password");
            Authentification authentification = Authentification.createInstance(classname, data, BinaryData.CreateInstance());
            Message message = Message.createInstance(call, id.ToString(), true, "");
            return MethodCallRequest.CreateInstance(authentification,message);
        }
        #endregion

    }
}
