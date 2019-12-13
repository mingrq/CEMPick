using CRMPick.Entity;
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

namespace CRMPick
{
    /// <summary>
    /// UpdatePwWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdatePwWindow : Window
    {

        private UserClass userclsaa;

        public UpdatePwWindow()
        {
            InitializeComponent();
            
        }

        public void SetUserEntity(UserClass userclsaa)
        {
            this.userclsaa = userclsaa;
        }

        private void UpdatePw(object sender, MouseButtonEventArgs e)
        {
            string userpwa = userpw.Text;
            string userpwagaina = userpwagain.Text;
            if (userpwa.Equals(""))
            {
                MessageBox.Show("请输入密码");
            }
            else if (userpwagaina.Equals(""))
            {
                MessageBox.Show("请再次输入密码");
            }
            else if (!userpwa.Equals(userpwagaina))
            {
                MessageBox.Show("两次输入的密码不同");
            }
            else
            {
                MysqlUtil mySql = new MysqlUtil();
                bool isupdate = mySql.updateUserPw(userclsaa.username,Encryption.GenerateMD5(userpwa));
                if (isupdate)
                {
                    string message = "密码修改成功!";
                    string caption = "提示";
                    // Show message box
                    MessageBoxResult result = MessageBox.Show(message, caption);
                    if (result == MessageBoxResult.OK)
                    {
                        this.Close();
                    }
                    
                }
                else
                {
                    MessageBox.Show("修改失败");
                }
            }
        }

        private void ContentRead(object sender, EventArgs e)
        {
            team.Content = userclsaa.team;
            username.Content = userclsaa.username;
            this.Topmost = false;
        }
    }
}
