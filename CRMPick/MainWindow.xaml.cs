using CRMPick.Entity;
using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.HtmlControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static CRMPick.Utils.IeVersionClass;

namespace CRMPick
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
       private UserClass user;
        public MainWindow(UserClass user)
        {
            InitializeComponent();
            this.user = user;
            IeVersionClass ieVersion = new IeVersionClass();
            ieVersion.SetIEVer(IeVersion.标准ie9);
        }

        /// <summary>
        /// 用户管理窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlOnClick(object sender, MouseButtonEventArgs e)
        {
            UserControlWindow userControlWindow = new UserControlWindow();
            userControlWindow.Topmost = true;
            userControlWindow.Show();
        }


        private void CRMOnClick(object sender, MouseButtonEventArgs e)
        {
            ALCRMWindow aLCRMWindow = new ALCRMWindow();
            aLCRMWindow.Topmost = true;
            aLCRMWindow.Show();

        }

        private void BatchTiaoRuOnClick(object sender, MouseButtonEventArgs e)
        {
            BatchTiaoRuWindow batchTiaoRuWindow = new BatchTiaoRuWindow();
            batchTiaoRuWindow.Topmost = true;
            batchTiaoRuWindow.Show();
        }

        private void BatchChaxunOnClick(object sender, MouseButtonEventArgs e)
        {
            BatchChaxunWindow batchChaxunWindow = new BatchChaxunWindow();
            batchChaxunWindow.Topmost = true;
            batchChaxunWindow.Show();
        }

        private void UserPWUpdateOnClick(object sender, MouseButtonEventArgs e)
        {
            UpdatePwWindow updatePwWindow = new UpdatePwWindow();
            updatePwWindow.SetUserEntity(user);
            updatePwWindow.Topmost = true;
            updatePwWindow.Show();
        }
    }
}
