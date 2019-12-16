using CRMPick.Entity;
using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                string teamname = team.Text.Trim();
                string userna = username.Text.Trim();
                string tiaorucount = tiaoruresourcecount.Text.Trim();
                string gathercount = gatherresourcecount.Text.Trim();

                if (teamname.Equals(""))
                {
                    MessageBox.Show("请输入使用团队");
                }
                else if (userna.Equals(""))
                {
                    MessageBox.Show("请输入用户名");
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
                    user.limited = limits;
                    user.tiaoruresourcecount = int.Parse(tiaorucount);
                    user.gatherresourcecount = int.Parse(gathercount);
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
        /// <summary>
        /// 限制只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restTime_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text);
        }
    }
}
