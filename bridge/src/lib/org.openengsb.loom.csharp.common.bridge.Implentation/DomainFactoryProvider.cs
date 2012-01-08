﻿/***
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
using System.IO;
using Org.Openengsb.Loom.Csharp.Common.Bridge.Interface;

namespace Org.OpenEngSB.Loom.Csharp.Common.Bridge.Impl
{
    public class DomainFactoryProvider
    {
        private static string CONFIGURATION_DIRECTORY = "conf";
        private static string CONFIGURATION_MOCK_FILE = "mocking.provider";

        public static IDomainFactory GetDomainFactoryInstance()
        {
            string mockFilePath = Path.Combine(CONFIGURATION_DIRECTORY, CONFIGURATION_MOCK_FILE);
            
            int version = 3;

            switch (version)
            {
                case (2): return new Org.OpenEngSB.Loom.Csharp.Common.Bridge.Impl.OpenEngSB2_0_0.RealDomainFactory();
                case (3): return new Org.OpenEngSB.Loom.Csharp.Common.Bridge.Impl.OpenEngSB3_0_0.RealDomainFactory();
            }
            return null;
        }
    }
}
