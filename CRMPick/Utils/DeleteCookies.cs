using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CRMPick.Utils
{
    class DeleteCookies
    {

        [DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetOption(int hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);
        public unsafe void SuppressWininetBehavior()
        {
            /* SOURCE: http://msdn.microsoft.com/en-us/library/windows/desktop/aa385328%28v=vs.85%29.aspx
            * INTERNET_OPTION_SUPPRESS_BEHAVIOR (81):
            * A general purpose option that is used to suppress behaviors on a process-wide basis.
            * The lpBuffer parameter of the function must be a pointer to a DWORD containing the specific behavior to suppress.
            * This option cannot be queried with InternetQueryOption.
            *
            * INTERNET_SUPPRESS_COOKIE_PERSIST (3):
            * Suppresses the persistence of cookies, even if the server has specified them as persistent.
            * Version: Requires Internet Explorer 8.0 or later.
            */
            int option = (int)3/* INTERNET_SUPPRESS_COOKIE_PERSIST*/;
            int* optionPtr = &option;
            bool success = InternetSetOption(0, 81/*INTERNET_OPTION_SUPPRESS_BEHAVIOR*/, new IntPtr(optionPtr), sizeof(int));
            ResetCookie();
            //Session的选项ID为42
            InternetSetOption(IntPtr.Zero, 42, IntPtr.Zero, 0);
        }
        
      
        //清空cookie
        public void ResetCookie()
        {
          
            string[] theCookies = System.IO.Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
            foreach (string currentFile in theCookies)
            {
                try
                {
                    System.IO.File.Delete(currentFile);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
