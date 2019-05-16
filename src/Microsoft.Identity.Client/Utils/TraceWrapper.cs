using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Utils
{
    internal class TraceWrapper
    {
        public static void WriteLine(string message) //TODO: move to pp?
        {
#if !NETSTANDARD && !WINDOWS_APP
            Trace.WriteLine(message);
#endif
        }

    }
}
