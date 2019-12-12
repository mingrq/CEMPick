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
    /// AddUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddUserWindow : Window
    {
        private bool isusernamerepeat = false;
        UserControlWindow userControlWindow;
        public AddUserWindow(UserControlWindow userControlWindow)
        {
            InitializeComponent();
            this.userControlWindow = userControlWindow;
        }

        private void AddUser(object sender, MouseButtonEventArgs e)
        {
            if (isusernamerepeat)
            {
                MessageBox.Show("用户名已存在，请修改用户名!!");
            }
            else
            {
                string teamname = team.Text;
                string userna = username.Text;
                string pw = userpw.Text;
                string pwaga = userpwagain.Text;
                if (teamname.Equals(""))
                {
                    MessageBox.Show("请输入使用团队");
                }
                else if (userna.Equals(""))
                {
                    MessageBox.Show("请输入用户名");
                }
                else if (pw.Equals(""))
                {
                    MessageBox.Show("请输入密码");
                }
                else if (pwaga.Equals(""))
                {
                    MessageBox.Show("请再次输入密码");
                }
                else if (!pw.Equals(pwaga))
                {
                    MessageBox.Show("两次输入的密码不一致");
                }
                else
                {
                    string limits = "1";
                    limits += pccb.IsChecked == true ? ",2" : "";
                    limits += ptcb.IsChecked == true ? ",3" : "";
                    limits += usercb.IsChecked == true ? ",4" : "";
                    UserClass user = new UserClass();
                    user.team = teamname;
                    user.username = userna;
                    user.userpw = pw;
                    user.limited = limits;
                    MysqlUtil mySql = new MysqlUtil();
                    bool isadd = mySql.addUser(user);
                    if (isadd)
                    {
                        userControlWindow.resh();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("添加失败");
                    }
                }
            }
        }

        private void usernameLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            string usernamem = textBox.Text;
            MysqlUtil mySql = new MysqlUtil();
            isusernamerepeat = mySql.getUserNameRepeat(usernamem);
            if (isusernamerepeat)
            {
                repeat.Visibility = Visibility.Visible;
            }
            else
            {
                repeat.Visibility = Visibility.Collapsed;
            }
        }
    }
}
