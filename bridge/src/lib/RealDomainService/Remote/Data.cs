using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    public class Data
    {
        public String username { get; set; }
        public String password { get; set; }

        public static Data CreateInstance(String username, String password)
        {
            Data instance = new Data();
            instance.username = username;
            instance.password = password;
            return instance;
        }
    }
}
