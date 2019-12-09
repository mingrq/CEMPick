using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CRMPick.Utils
{
    class CacheImage
    {
        [DllImport("Wininet.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern Boolean GetUrlCacheEntryInfo(String lpxaUrlName, IntPtr lpCacheEntryInfo, ref int lpdwCacheEntryInfoBufferSize);
        const int ERROR_FILE_NOT_FOUND = 0x2;
        struct LPINTERNET_CACHE_ENTRY_INFO
        {
            public int dwStructSize;
            IntPtr lpszSourceUrlName;
            public IntPtr lpszLocalFileName;
            // int CacheEntryType;
            // int dwUseCount;
            // int dwHitRate;
            //int dwSizeLow;
            // int dwSizeHigh;

            //System.Runtime.InteropServices.ComTypes.FILETIME LastModifiedTime;
            //System.Runtime.InteropServices.ComTypes.FILETIME Expiretime;
            // System.Runtime.InteropServices.ComTypes.FILETIME LastAccessTime;
            //System.Runtime.InteropServices.ComTypes.FILETIME LastSyncTime;
            //IntPtr lpHeaderInfo;
            //readonly int dwheaderInfoSize;
            //IntPtr lpszFileExtension;
            //int dwEemptDelta;
        }
        // 返回 指定URL文件的缓存在本地文件系统中的路径
        public string GetPathForCachedFile(string fileUrl)
        {
            int cacheEntryInfoBufferSize = 0;
            IntPtr cacheEntryInfoBuffer = IntPtr.Zero;
            int lastError; Boolean result;
            try
            {
                result = GetUrlCacheEntryInfo(fileUrl, IntPtr.Zero, ref cacheEntryInfoBufferSize);
                lastError = Marshal.GetLastWin32Error();
                if (result == false)
                {
                    if (lastError == ERROR_FILE_NOT_FOUND)
                        return null;
                }
                cacheEntryInfoBuffer = Marshal.AllocHGlobal(cacheEntryInfoBufferSize);
                result = GetUrlCacheEntryInfo(fileUrl, cacheEntryInfoBuffer, ref cacheEntryInfoBufferSize);
                lastError = Marshal.GetLastWin32Error();
                if (result == true)
                {
                    Object strObj = Marshal.PtrToStructure(cacheEntryInfoBuffer, typeof(LPINTERNET_CACHE_ENTRY_INFO));
                    LPINTERNET_CACHE_ENTRY_INFO internetCacheEntry = (LPINTERNET_CACHE_ENTRY_INFO)strObj;
                    String localFileName = Marshal.PtrToStringAuto(internetCacheEntry.lpszLocalFileName); return localFileName;
                }
                else
                    return null;// file not found
            }
            finally
            {
                if (!cacheEntryInfoBuffer.Equals(IntPtr.Zero))
                    Marshal.FreeHGlobal(cacheEntryInfoBuffer);
            }
        }
    }
}
