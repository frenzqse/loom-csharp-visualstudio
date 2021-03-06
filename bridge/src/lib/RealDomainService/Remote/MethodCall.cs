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

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// This class represents a RPC with its parameters, return types etc.
    /// </summary>
    public class MethodCall
    {
        /// <summary>
        /// Fully qualified class names of the arguments.
        /// </summary>
        public IList<string> classes { get; set; }
        /// <summary>
        /// Name of the method to be called.
        /// </summary>
        public string methodName { get; set; }
        
        /// <summary>
        /// Arguments of the call.
        /// </summary>
        public IList<object> args { get; set; }

        /// <summary>
        /// Metadata
        /// </summary>
        public IDictionary<string, string> metaData { get; set; }
        /// <summary>
        /// Include the packagestruktur on the java side
        /// </summary>
        public IList<string> realClassImplementation { get; set; }

        public static MethodCall CreateInstance(string methodName, IList<object> args, IDictionary<string, string> metaData, IList<string> classes, IList<String> realClassImplementation)
        {
            MethodCall call = new MethodCall();
            call.methodName = methodName;
            call.args = args;
            call.metaData = metaData;
            call.classes = classes;
            call.realClassImplementation = realClassImplementation;
            return call;
        }
    }
}
