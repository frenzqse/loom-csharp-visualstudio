using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    public class Message
    {
        public MethodCall methodCall { get; set; }
        public string callId { get; set; }
        public bool answer { get; set; }
        public string destination { get; set; }

        public static Message createInstance(MethodCall methodCall, string callId, bool answer, string destination)
        {
            Message instance = new Message();
            instance.methodCall = methodCall;
            instance.callId = callId;
            instance.answer = answer;
            instance.destination = destination;
            return instance;
        }
    }
}
