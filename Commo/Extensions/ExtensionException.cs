using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commo.Extensions
{
    public static class ExtensionException
    {
        public static Exception GetFirstException(this Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex;
            } // end case
            else
            {
                return GetFirstException(ex.InnerException);
            } // recurse
        }
    }
}
