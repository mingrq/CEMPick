using CRMPick.Entity;
using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace CRMPick
{
    /// <summary>
    /// UserControlWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlWindow : Window
    {
        public UserControlWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUser(object sender, MouseButtonEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
        }

        
        private void WinLoaded(object sender, EventArgs e)
        {
            MysqlUtil mysqlUtil = new MysqlUtil();
            List<UserClass> users = mysqlUtil.getUserList();
            list.ItemsSource = users;            
        }
    }


}
