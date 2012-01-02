using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote.RemoteObjects
{
    public class SecureMethodCallRequest
    {
        public String principal { get; set; }
        public AuthenticationInfo credentials { get; set; }
        public long timestamp { get; set; }
        public Message message { get; set; }

        public static SecureMethodCallRequest createInstance(String principal, AuthenticationInfo credentials, Message message)
        {
            SecureMethodCallRequest instance = new SecureMethodCallRequest();
            instance.principal = principal;
            instance.credentials = credentials;
            instance.timestamp = DateTime.Now.Ticks;
            instance.message = message;
            return instance;
        }
    }
}
