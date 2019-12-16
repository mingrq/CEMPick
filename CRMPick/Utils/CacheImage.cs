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

namespace CRMPick.Utils
{
    class CacheImage
    {

        public BitmapSource GetCacheImage(WebBrowser webBrower, string id)
        {
            //string strVcodeUser = ConfigurationManager.AppSettings["chaorenuser"];
            //string strVcodePass = ConfigurationManager.AppSettings["chaorenpw"];
            //string strSoftId = ConfigurationManager.AppSettings["66785"];


            HTMLDocument doc = (HTMLDocument)webBrower.Document;
            HTMLBody body = (HTMLBody)doc.body;

            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement img = (IHTMLControlElement)(doc.getElementById(id));
            rang.add(img);
            rang.execCommand("Copy", true, null);
            BitmapSource bitmap = Clipboard.GetImage();
           // byte[] dcimg = BitmapSourceToArray(bitmap);//验证码字节码
          
           
            //Image regImg = new Image();
            //regImg.Source = bitmap;
            //Clipboard.Clear();

            //HTMLDocument doc = (HTMLDocument)webBrower.Document;
            //HTMLBody body = (HTMLBody)doc.body;
            //IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            //IHTMLControlElement img = (IHTMLControlElement)(body.children[0]);
            //rang.add(img);
            //rang.execCommand("Copy", true, null);
            //BitmapSource bitmap = Clipboard.GetImage();
            //Image regImg = new Image();
            //regImg.Source = bitmap;
            Clipboard.Clear();
            //string verificationCode = Dc.RecByte_A(dcimg, imglen, ConfigurationManager.AppSettings["chaorenuser"], ConfigurationManager.AppSettings["chaorenpw"], ConfigurationManager.AppSettings["66785"]);
            //string verificationCode = Dc.RecByte_A(dcimg, imglen, ConfigurationManager.AppSettings["chaorenuser"], ConfigurationManager.AppSettings["chaorenpw"], ConfigurationManager.AppSettings["softid"]);

            return bitmap;
        }

        public byte[] BitmapSourceToArray(BitmapSource bitmapSource)
        {
            int height = bitmapSource.PixelHeight;
            int width = bitmapSource.PixelWidth;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            byte[] bits = new byte[height * stride + 12];
            bitmapSource.CopyPixels(bits, stride, 0);
            bits[bits.Length - 12] = (byte)(width >> 24);
            bits[bits.Length - 11] = (byte)((width << 8) >> 24);
            bits[bits.Length - 10] = (byte)((width << 16) >> 24);
            bits[bits.Length - 9] = (byte)(width & 255);

            bits[bits.Length - 8] = (byte)(height >> 24);
            bits[bits.Length - 7] = (byte)((height << 8) >> 24);
            bits[bits.Length - 6] = (byte)((height << 16) >> 24);
            bits[bits.Length - 5] = (byte)(height & 255);

            bits[bits.Length - 4] = (byte)(stride >> 24);
            bits[bits.Length - 3] = (byte)((stride << 8) >> 24);
            bits[bits.Length - 2] = (byte)((stride << 16) >> 24);
            bits[bits.Length - 1] = (byte)(stride & 255);
            return bits;
        }

        public BitmapSource ArrayToBitmapSource(byte[] imageBytes)
        {
            PixelFormat pf = PixelFormats.Bgra32;
            int width = (imageBytes[imageBytes.Length - 12] << 24)
                       + (imageBytes[imageBytes.Length - 11] << 16)
                       + (imageBytes[imageBytes.Length - 10] << 8)
                       + (imageBytes[imageBytes.Length - 9]);

            int height = (imageBytes[imageBytes.Length - 8] << 24)
                       + (imageBytes[imageBytes.Length - 7] << 16)
                       + (imageBytes[imageBytes.Length - 6] << 8)
                       + (imageBytes[imageBytes.Length - 5]);

            int rawStride = (imageBytes[imageBytes.Length - 4] << 24)
                          + (imageBytes[imageBytes.Length - 3] << 16)
                          + (imageBytes[imageBytes.Length - 2] << 8)
                          + (imageBytes[imageBytes.Length - 1]);


            List<Byte> tempList = imageBytes.ToList();
            tempList.RemoveRange(imageBytes.Length - 12, 12);
            imageBytes = tempList.ToArray();
            BitmapSource bmpImage = BitmapSource.Create(width, height,
                96, 96, pf, null,
                imageBytes, rawStride);
            return bmpImage;
        }
    }
}
