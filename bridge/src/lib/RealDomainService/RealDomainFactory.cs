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
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote;
using Org.OpenEngSB.DotNet.Lib.RealDomainService.Common;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService
{
    /// <summary>
    /// This class produces and manages proxies.
    /// </summary>
    public class RealDomainFactory : IDomainFactory
    {
        private Dictionary<object, IStoppable> _proxies;

        public RealDomainFactory()
        {
            Reset();
        }

        private void Reset()
        {
            _proxies = new Dictionary<object, IStoppable>();
        }

        /// <summary>
        /// Creates, registers and starts a reverse proxy.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="destination"></param>
        /// <param name="domainService"></param>
        /// <param name="serviceId"></param>
        /// <param name="domainType"></param>
        public string RegisterDomainService<T>(string destination, T domainService, string domainType)
        {
            string servideId = Guid.NewGuid().ToString();
            DomainReverseProxy<T> proxy = new DomainReverseProxy<T>(domainService, destination, servideId, domainType);
            _proxies.Add(domainService, proxy);
            proxy.Start();
            return servideId;
        }

        /// <summary>
        /// Deletes and stops the reverse proxy.
        /// </summary>
        /// <param name="service"></param>
        public void UnregisterDomainService(object service)
        {
            IStoppable stoppable = null;
            if(_proxies.TryGetValue(service, out stoppable))
            {
                stoppable.Stop();
                _proxies.Remove(service);
            }
        }

        public T RetrieveDomainProxy<T>(string host, string serviceId)
        {
            return new DomainProxy<T>(host, serviceId).GetTransparentProxy();
        }
    }
}
