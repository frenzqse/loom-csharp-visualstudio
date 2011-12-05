using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    public class MessageResult
    {
        public MethodResult result { get; set; }
        public string callId { get; set; }

        public static MessageResult CreateInstance(MethodResult result, string callId)
        {
            MessageResult msg = new MessageResult();
            msg.result = result;
            msg.callId = callId;
            return msg;
        }
    }
}
