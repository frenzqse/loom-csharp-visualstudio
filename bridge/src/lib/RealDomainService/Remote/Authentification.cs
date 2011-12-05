using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService.Remote
{
    public class Authentification
    {
        public String className { get; set; }
        public Data data { get; set; }
        public BinaryData binaryData { get; set; }

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
