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
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 登录按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string usernamee = username.Text.Trim();
            string pw = userpw.Password.Trim();
            if (usernamee.Equals(""))
            {
                MessageBox.Show("请输入用户名！");
                return;
            }
            else if (pw.Equals(""))
            {
                MessageBox.Show("请输入密码！");
                return;
            }
            else
            {
                MysqlUtil mySqlUtil = new MysqlUtil();
                UserClass user = mySqlUtil.getUser(usernamee);
                if (user != null)
                {
                    if (pw.Equals(user.userpw))
                    {
                        MysqlUtil mysqlUtil = new MysqlUtil();
                        string uuid = UUIDClass.GetUUID();
                        if (!user.facility.Equals(""))
                        {
                            if (user.facility.Equals(uuid))
                            {
                                MainWindow mainWindow = new MainWindow();
                                mainWindow.Show();
                                this.Close();
                            }
                            else
                            {
                                if (!user.facilitytwo.Equals(""))
                                {
                                    if (user.facilitytwo.Equals(uuid))
                                    {
                                        MainWindow mainWindow = new MainWindow();
                                        mainWindow.Show();
                                        this.Close();
                                    }
                                    else
                                    {
                                        MessageBox.Show("设备不允许使用此工具！");
                                    }
                                }
                                else
                                {
                                    mysqlUtil.updateUUID(uuid, "facilitytwo", user.username);
                                    MainWindow mainWindow = new MainWindow();
                                    mainWindow.Show();
                                    this.Close();
                                }
                            }

                        }
                        else
                        {
                            mysqlUtil.updateUUID(uuid, "facility", user.username);
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }

                    }
                    else
                    {
                        MessageBox.Show("密码错误");
                    }
                }
                else
                {
                    MessageBox.Show("用户不存在");
                }
            }
        }


        string FouseContent;
        /// <summary>
        /// 失去焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Trim().Equals(""))
            {
                textBox.Text = FouseContent;
            }
        }

        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text.Equals("用户名") || textBox.Text.Equals("用户密码"))
            {
                FouseContent = textBox.Text;
                textBox.Text = "";
            }
        }
    }
}
