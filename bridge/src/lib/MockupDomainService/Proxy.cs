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
using Org.OpenEngSB.DotNet.Lib.DomainService;
using System.Reflection;
using Org.OpenEngSB.DotNet.Lib.MockupDomainService.MonitorService;
using System.Xml.Serialization;
using System.IO;

namespace Org.OpenEngSB.DotNet.Lib.MockupDomainService
{
    public class Proxy<T> : RealProxy
    {
        private MonitorServiceClient _monitor;

        public Dictionary<MethodInfo, int> IDs { get; set; }

        public Proxy(MonitorServiceClient monitor, Dictionary<MethodInfo, int> ids = null) : base(typeof(T))
        {
            _monitor = monitor;
            IDs = ids;
        }

        public override IMessage Invoke(IMessage msg)
        {
            object retValue = default(T);
            MessageMethodInvocation invocation = new MessageMethodInvocation(msg);

            // Implements basic object methods, mostly by using the type directly
            if (invocation.TargetType == typeof(object))
            {
                switch (invocation.MethodName)
                {
                    case "GetType":
                        retValue = typeof(T);
                        break;
                    case "Equals":
                        if (invocation.Arguments[0] == null)
                            retValue = false;
                        else
                            retValue = typeof(T).Equals(invocation.Arguments[0].GetType());
                        break;
                    default:
                        retValue = invocation.Invoke(typeof(T));
                        break;
                }
            }
            else // Impelenting the real mockup proxy
            {
                //string strRet = _mockupData.GetMockupData(invocation.TargetType, invocation.Method, invocation.Arguments);
                //retValue = ConvertString(strRet, invocation.Method.ReturnType);

                XmlSerializer serializer;
                object[] parameters = new object[invocation.Arguments.Length];

                for (int i = 0; i < parameters.Length; i++)
                {
                    serializer = new XmlSerializer(invocation.MethodSignature[i]);
                    StringWriter sw = new StringWriter();

                    serializer.Serialize(sw, invocation.Arguments[i]);
                    parameters[i] = sw.ToString();
                }

                retValue = _monitor.MethodExecuted(IDs[invocation.Method], parameters);

                if (retValue != null)
                {
                    serializer = new XmlSerializer(invocation.Method.ReturnType);
                    StringReader sr = new StringReader(retValue.ToString());

                    retValue = serializer.Deserialize(sr);
                }
            }

            return invocation.ReturnValue(retValue);
        }

        private object ConvertString(string val, Type targetType)
        {
            object ret = val;
            bool isNullable = targetType.IsValueType;

            if (targetType == typeof(void))
                ret = null;
            else if (targetType != typeof(string))
            {
                if (targetType == typeof(Nullable<>))
                    targetType = targetType.GetGenericArguments()[0];

                if (isNullable && val == null)
                    return null;
                else
                {
                    Type convertType = typeof(Convert);
                    MethodInfo mi = convertType.GetMethod(string.Format("To{0}", targetType.Name), BindingFlags.Static | BindingFlags.Public, null, new Type[] { typeof(string) }, null);

                    if (mi != null)
                        ret = mi.Invoke(null, new object[] { val });
                    else
                        throw new NotImplementedException();
                }
            }

            return ret;
        }

        public new T GetTransparentProxy()
        {
            return (T)base.GetTransparentProxy();
        }
    }
}
