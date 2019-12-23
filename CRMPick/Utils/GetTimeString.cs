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

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return ts.TotalSeconds.ToString();
        }


        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToLong(DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long getNowTime()
        {
            return ConvertDateTimeToLong(DateTime.Now.ToLocalTime());
        }

        public static long getSetTime(string time)
        {
            DateTime dt;
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd HH:mm:ss";
            dt = Convert.ToDateTime(time, dtFormat);
            return ConvertDateTimeToLong(dt);
        }
    }
}
