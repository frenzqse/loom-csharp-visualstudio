using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// Container for the datas
    /// </summary>
    public class Data
    {
        public String username { get; set; }
        public String password { get; set; }

        /// <summary>
        /// Creates a new instance of Data
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns>New instance of Data</returns>
        public static Data CreateInstance(String username, String password)
        {
            Data instance = new Data();
            instance.username = username;
            instance.password = password;
            return instance;
        }
    }
}
