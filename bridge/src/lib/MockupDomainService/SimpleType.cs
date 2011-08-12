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
using System.Reflection;

namespace Org.OpenEngSB.DotNet.Lib.MockupDomainService.MonitorService
{
    public partial class SimpleType
    {
        public SimpleType() { }

        public SimpleType(Type baseType)
        {
            if (baseType != null)
            {
                Type typeOfType = typeof(Type);

                foreach (var prop in GetType().GetProperties())
                {
                    PropertyInfo pi = typeOfType.GetProperty(prop.Name);

                    if (pi != null)
                    {
                        if (pi.PropertyType == prop.PropertyType)
                            prop.SetValue(this, pi.GetValue(baseType, null), null);
                        else if (prop.PropertyType == typeof(SimpleType) && pi.PropertyType == typeof(Type))
                            prop.SetValue(this, new SimpleType((Type)pi.GetValue(baseType, null)), null);
                        else
                            throw new NotImplementedException();
                    }
                }
            }
        }
    }
}
