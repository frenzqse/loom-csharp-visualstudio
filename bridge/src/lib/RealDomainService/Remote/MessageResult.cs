using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// Container for MessageResult
    /// </summary>
    public class MessageResult
    {
        public MethodResult result { get; set; }
        public string callId { get; set; }

        /// <summary>
        /// Creates a new instance of MessageResult
        /// </summary>
        /// <param name="result">Result</param>
        /// <param name="callId">CallId</param>
        /// <returns>returns a new instance of MessageResult</returns>
        public static MessageResult CreateInstance(MethodResult result, string callId)
        {
            MessageResult msg = new MessageResult();
            msg.result = result;
            msg.callId = callId;
            return msg;
        }
    }
}
