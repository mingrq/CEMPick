using CRMPick.Utils;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static CRMPick.Utils.IeVersionClass;

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

    }
}
