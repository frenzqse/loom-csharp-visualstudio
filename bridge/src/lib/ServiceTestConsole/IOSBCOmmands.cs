using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org.OpenEngSB.DotNet.Lib.RealDomainService
{
    interface IOSBCOmmands
    {
        Object raiseEvent(String name, String lastKnowVersion, String query, String origin);
    }
}
