using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using mshtml;
using System.Windows;
using static CRMPick.Utils.IeVersionClass;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Net;
using System.Management;
using System.Configuration;
using DcVerCode;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CRMPick.Utils
{
    class CacheImage
    {

        public string GetCacheImage(WebBrowser webBrower, string id)
        {
            HTMLDocument doc = (HTMLDocument)webBrower.Document;
            HTMLBody body = (HTMLBody)doc.body;

            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement img = (IHTMLControlElement)(doc.getElementById(id));
            rang.add(img);
            Clipboard.Clear();
            rang.execCommand("Copy", true, null);
            BitmapSource bitmap = Clipboard.GetImage();
            Clipboard.Clear();
            byte[] dcimg = ConvertToBytes(bitmap);//验证码字节码
            string result = Dc.RecByte_A(dcimg, dcimg.Length, ConfigurationManager.AppSettings["chaorenuser"], ConfigurationManager.AppSettings["chaorenpw"], ConfigurationManager.AppSettings["softid"]);
            string verificationCode =result.Substring(0, result.IndexOf("|!|"));
            return verificationCode;
        }

        /// <summary>
        /// 将bitmapSource转byte[]
        /// </summary>
        /// <param name="bitmapSource"></param>
        /// <returns></returns>
        private byte[] ConvertToBytes(BitmapSource bitmapSource)
        {
            byte[] buffer = null;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);
            memoryStream.Position = 0;
            if (memoryStream.Length > 0)
            {
                using (BinaryReader br = new BinaryReader(memoryStream))
                {
                    buffer = br.ReadBytes((int)memoryStream.Length);
                }
            }
            memoryStream.Close();
            return buffer;
        }
    }
}
