using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// Container for Message
    /// </summary>
    public class Message
    {
        public MethodCall methodCall { get; set; }
        public string callId { get; set; }
        public bool answer { get; set; }
        public string destination { get; set; }

        /// <summary>
        /// Creates a new instance of Message
        /// </summary>
        /// <param name="methodCall">MethodCall</param>
        /// <param name="callId">CallId</param>
        /// <param name="answer">Answer</param>
        /// <param name="destination">Destination</param>
        /// <returns>returns a new Message of CreateInstance</returns>
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
