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
using Org.OpenEngSB.DotNet.Lib.DomainService;
using System.Windows;
using System.Threading;
using Org.OpenEngSB.DotNet.Lib.MockupDomainService.MonitorService;
using System.ServiceModel;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;

namespace Org.OpenEngSB.DotNet.Lib.MockupDomainService
{
    public class DomainFactory : IDomainFactory, IMonitorServiceCallback, IDisposable
    {
        private static DomainFactory _me;

        private int _objectId;
        private object _objectIdLocker = new object();

        private Dictionary<int, AllMethodInfos> _methods = new Dictionary<int, AllMethodInfos>();
        private MonitorServiceClient _monitor;

        private DomainFactory()
        {
            InstanceContext ic = new InstanceContext(this);
            _monitor = new MonitorServiceClient(ic);
        }

        public static DomainFactory Instance
        {
            get
            {
                if (_me == null)
                {
                    _me = new DomainFactory();
                }

                return _me;
            }
        }

        public T RetrieveDomainProxy<T>(string host, string serviceId)
        {
            Proxy<T> proxy = new Proxy<T>(_monitor);
            T ret = proxy.GetTransparentProxy();

            var ids = AddAllMethods(typeof(T), MethodType.Retrieved, ret);
            proxy.IDs = ids;

            return ret;
        }

        public string RegisterDomainService<T>(string destination, T service, string domainType)
        {
            AddAllMethods(typeof(T), MethodType.Registered, service);
            return "";
        }

        private Dictionary<MethodInfo, int> AddAllMethods(Type type, MethodType methodType, object implObject)
        {
            try
            {
                Dictionary<MethodInfo, int> ret = new Dictionary<MethodInfo, int>();
                int id;

                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    id = GetNewID(implObject, methodType, method, type);
                    ret.Add(method, id);

                    _monitor.AddMethod(id, new SimpleMethodInfo(method), methodType);
                }

                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error communicating with the monitor.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }

        private int GetNewID(object implObject, MethodType methodType, MethodInfo method, Type type)
        {
            int myObjectId;

            lock (_objectIdLocker)
            {
                myObjectId = _objectId++;
            }

            _methods.Add(myObjectId, new AllMethodInfos() { ID = myObjectId, ImplObject = implObject, Type = methodType, BaseType = type, Info = method });

            return myObjectId;
        }

        public object ExecuteMethod(int id, object[] parameters)
        {
            var method = _methods[id];
            ParameterInfo[] parameterDefinitions = method.Info.GetParameters();

            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameterDefinitions[i].ParameterType.IsEnum)
                {
                    parameters[i] = Enum.Parse(parameterDefinitions[i].ParameterType, parameters[i].ToString());
                }
                else if (!parameterDefinitions[i].ParameterType.IsPrimitive)
                {
                    XmlSerializer serializer = new XmlSerializer(parameterDefinitions[i].ParameterType);

                    // just test if the object is ok
                    parameters[i] = serializer.Deserialize(new StringReader(parameters[i].ToString()));

                }
            }

            return method.Info.Invoke(method.ImplObject, parameters);
        }

        public void Dispose()
        {
            if (_monitor != null)
            {
                _monitor.Unsubscribe();
                _monitor = null;
            }
        }


        public void UnregisterDomainService(object service)
        {
            throw new NotImplementedException();
        }
    }
}
