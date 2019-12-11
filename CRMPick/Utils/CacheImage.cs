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

namespace CRMPick.Utils
{
    class CacheImage
    {
       public void GetCacheImage(WebBrowser webBrower, dynamic elem)
        {
            HTMLDocument doc = (HTMLDocument)webBrower.Document;
            HTMLBody body = (HTMLBody)doc.body;

            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement img = (IHTMLControlElement)(elem);
            rang.add(img);
            rang.execCommand("Copy", true, null);
            BitmapSource bitmap = Clipboard.GetImage();
            Image regImg = new Image();
            regImg.Source = bitmap;
            Clipboard.Clear();

            //HTMLDocument doc = (HTMLDocument)webBrower.Document;
            //HTMLBody body = (HTMLBody)doc.body;
            //IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            //IHTMLControlElement img = (IHTMLControlElement)(body.children[0]);
            //rang.add(img);
            //rang.execCommand("Copy", true, null);
            //BitmapSource bitmap = Clipboard.GetImage();
            //Image regImg = new Image();
            //regImg.Source = bitmap;
            ////Clipboard.Clear();
        }
    }
}
