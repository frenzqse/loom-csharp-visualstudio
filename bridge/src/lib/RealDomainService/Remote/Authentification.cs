using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    /// <summary>
    /// Container for the Authentification
    /// </summary>
    public class Authentification
    {
        public String className { get; set; }
        public Data data { get; set; }
        public BinaryData binaryData { get; set; }

        /// <summary>
        /// Creates a new instance of the Authentification
        /// </summary>
        /// <param name="className">ClassName</param>
        /// <param name="data">Data</param>
        /// <param name="binaryData">Binary Data</param>
        /// <returns>A new instance of Authentification</returns>
        public static Authentification createInstance(String className, Data data, BinaryData binaryData)
        {
            Authentification instance = new Authentification();
            instance.className = className;
            instance.data = data;
            instance.binaryData = binaryData;
            return instance;
        }
    }
}
