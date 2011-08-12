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
    public class MethodCallRequest
    {
        public MethodCall methodCall { get; set; }
        public string callId { get; set; }
        public bool answer { get; set; }
        public string destination { get; set; }

        public static MethodCallRequest CreateInstance(MethodCall methodCall, string callId, bool answer, string destination)
        {
            MethodCallRequest request = new MethodCallRequest();
            request.methodCall = methodCall;
            request.callId = callId;
            request.answer = answer;
            request.destination = destination;
            return request;
        }
    }
}
