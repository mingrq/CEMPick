using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using mshtml;
using System.Windows;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Net;
using System.Management;
using System.Configuration;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CRMPick.Utils
{
    class CacheImage
    {
        public delegate void MyDelegate(string verificationCode);

        public void GetCacheImage(WebBrowser webBrower, string id, MyDelegate myDelegate, Window window)
        {

            HTMLDocument doc = (HTMLDocument)webBrower.Document;
            HTMLBody body = (HTMLBody)doc.body;

            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement img = (IHTMLControlElement)(doc.getElementById(id));
            rang.add(img);
            rang.execCommand("Copy", true, null);
            try
            {
                BitmapSource bitmap = Clipboard.GetImage();
                byte[] dcimg = ConvertToBytes(bitmap);//验证码字节码
                Clipboard.Clear();
                string result = Dc.RecByte_A(dcimg, dcimg.Length, ConfigurationManager.AppSettings["chaorenuser"], ConfigurationManager.AppSettings["chaorenpw"], ConfigurationManager.AppSettings["softid"]);
                string verificationCode = result.Substring(0, result.IndexOf("|!|"));
                myDelegate(verificationCode);
                doc = null;
                body = null;
                rang = null;
                img = null;
                GC.Collect();
            }
            catch (Exception e)
            {
                
                Thread thr = new Thread(() =>
                {
                    string userSaveFiler = Directory.GetCurrentDirectory() + "\\weeorlog.txt";//用户账号保存文件
                    FileStream fs = new FileStream(userSaveFiler, FileMode.OpenOrCreate);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(e.ToString());
                    sw.Close();
                    fs.Close();
                    //这里还可以处理些比较耗时的事情。
                    Thread.Sleep(1000);//延时2秒
                    window.Dispatcher.Invoke(new Action(() =>
                    {
                        Clipboard.Clear();
                        GetCacheImage(webBrower, id, myDelegate, window);
                    }));
                });
                thr.Start();
            }
        }

        private void GetCacheImage(WebBrowser webBrower, string id)
        {
            throw new NotImplementedException();
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
