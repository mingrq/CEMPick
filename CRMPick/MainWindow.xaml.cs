using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMPick
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 用户管理窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlOnClick(object sender, MouseButtonEventArgs e)
        {
            UserControlWindow userControlWindow = new UserControlWindow();
            userControlWindow.Show();
        }


        private void CRMOnClick(object sender, MouseButtonEventArgs e)
        {
            ALCRMWindow aLCRMWindow = new ALCRMWindow();
            aLCRMWindow.Show();
            aLCRMWindow.Topmost = true;
        }

        private void BatchTiaoRuOnClick(object sender, MouseButtonEventArgs e)
        {
            BatchTiaoRuWindow batchTiaoRuWindow = new BatchTiaoRuWindow();
            batchTiaoRuWindow.Show();
        }

        private void BatchChaxunOnClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache));
            CacheImage i = new CacheImage();
            //在网页上找到百度logo图片的url写在下面
            string b = i.GetPathForCachedFile("get_img?identity=caenir.alibaba-inc.com&sessionid=k0Ig5XK02mr2iNsIMcdjs5gdn6kU7h1Ua+6vxiE8HU3so=&timestamp=407");
            Console.WriteLine(b);
            
            
        }
    }
}
