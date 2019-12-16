using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Utils
{
    class GetTimeString
    {
        public static string gettime()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
