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
using Org.OpenEngSB.DotNet.Lib.RealDomainService;
using Org.OpenEngSB.DotNet.Lib.DomainService;

namespace ServiceManager.Core
{
    public class ServiceConnectorManager
    {
        private static string CONFIG_DIRECTORY = "conf";
        private enum State { CONNECTED, DISCONNECTED }
        private State _State;

        public bool IsConnected { get { return _State == State.CONNECTED; } }

        public IList<ServiceConfiguration> _configurations;

        IDomainFactory _domainFactory;

        public ServiceConnectorManager()
        {
            _configurations = null;
            _domainFactory = DomainFactoryProvider.GetDomainFactoryInstance();
            _State = State.DISCONNECTED;
        }

        public void Connect()
        {
            if (_State == State.CONNECTED)
                return;

            if (_configurations == null)
                LoadConfigurations();

            RegisterServices();

            _State = State.CONNECTED;
        }

        private void LoadConfigurations()
        {
            ConfigurationDirectory dir = new ConfigurationDirectory(CONFIG_DIRECTORY);
            _configurations = dir.LoadConfigurations();
        }

        private void RegisterServices()
        {
            if (_configurations == null)
                return;

            foreach (ServiceConfiguration config in _configurations)
            {
                _domainFactory.RegisterDomainService(config.DestinationString,
                    config.GetServiceInstance(),
                    config.DomainType);
            }
        }

        private void UnregisterServices()
        {
            if (_configurations == null)
                return;

            foreach (ServiceConfiguration config in _configurations)
            {
                _domainFactory.UnregisterDomainService(config.GetServiceInstance());
            }
        }

        public void Disconnect()
        {
            if (_State == State.DISCONNECTED)
                return;

            if (_configurations == null)
                return;

            UnregisterServices();

            _State = State.DISCONNECTED;
        }
    }
}
