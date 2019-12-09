using CRMPick.Utils;
using System;
using mshtml;
using System.Windows;
using static CRMPick.Utils.IeVersionClass;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Net;
using System.Management;

namespace CRMPick
{
    /// <summary>
    /// ALCRMWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ALCRMWindow : Window
    {

        public ALCRMWindow()
        {
            InitializeComponent();
            IeVersionClass ieVersion = new IeVersionClass();
            ieVersion.SetIEVer(IeVersion.标准ie11);
            webBrower.Source = new Uri("https://pin.aliyun.com/get_img?identity=caenir.alibaba-inc.com&sessionid=k0Ig5XK02mr2iNsIMcdjs5gUDnfigit+IDUY+GT/Bn32Q=");
            this.ContentRendered += MLoad;
        }

        private void MLoad(object sender, EventArgs e)
        {
            this.Topmost = false;
        }

        private void TbLostF(object sender, RoutedEventArgs e)
        {
            if (tbresouses.Text.Trim().Equals(""))
            {
                tbresouses.Text = "将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息";
            }
        }

        private void TbGotF(object sender, RoutedEventArgs e)
        {
            if (tbresouses.Text.Trim().Equals("将客户资源复制到文本框中，点击查询，每点击一次查询自动搜索出该公司信息"))
            {
                tbresouses.Text = "";
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HTMLDocument doc = (HTMLDocument)webBrower.Document;
            HTMLBody body = (HTMLBody)doc.body;
            IHTMLControlRange rang = (IHTMLControlRange)body.createControlRange();
            IHTMLControlElement img = (IHTMLControlElement)(body.children[0]);
            rang.add(img);
            rang.execCommand("Copy", true, null);
            BitmapSource bitmap = Clipboard.GetImage();
            Image regImg = new Image();
            regImg.Source = bitmap;
            Clipboard.Clear();
         
        }
           
    }
}
