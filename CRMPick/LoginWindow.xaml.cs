using CRMPick.Entity;
using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private string userSaveFiler = Directory.GetCurrentDirectory() + "\\accountinfo.txt";//用户账号保存文件
        private bool ischeck;//是否记住密码
        string usernameep;//登录用户名
        string pwp;//登录用户密码
        public LoginWindow()
        {
            InitializeComponent();
            if (File.Exists(userSaveFiler))
            {
                //账号信息文件存在
                StreamReader reader = new StreamReader(userSaveFiler);
                string acce = reader.ReadToEnd();
                if (!acce.Trim().Equals(""))
                {
                    string[] accearray = acce.Split('&');
                    username.Text = StringSecurity.DESDecrypt(accearray[0]);
                    if (!accearray[1].Trim().Equals(""))
                    {
                        userpw.Password = StringSecurity.DESDecrypt(accearray[1]);
                        jizhumima.IsChecked = true;
                    }
                    reader.Close();
                }
            }
        }


        /// <summary>
        /// 登录按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string usernamee = usernameep = username.Text.Trim();
            string pw = pwp = userpw.Password.Trim();
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
                pw = Encryption.GenerateMD5(pw);
                MysqlUtil mySqlUtil = new MysqlUtil();
                UserClass user = mySqlUtil.getUser(usernamee);
                if (user != null)
                {
                    if (pw.Equals(user.userpw))
                    {
                        MysqlUtil mysqlUtil = new MysqlUtil();
                        /**
                         *管理员登录不进行uuid对比
                         */
                        if (user.username.Equals("admin"))
                        {
                            LoginSuccess(mySqlUtil, user);
                        }
                        else
                        {
                            string uuid = UUIDClass.GetUUID();
                            if (!user.facility.Equals(""))
                            {
                                if (user.facility.Equals(uuid))
                                {
                                    LoginSuccess(mySqlUtil, user);
                                }
                                else
                                {
                                    if (!user.facilitytwo.Equals(""))
                                    {
                                        if (user.facilitytwo.Equals(uuid))
                                        {
                                            LoginSuccess(mySqlUtil, user);
                                        }
                                        else
                                        {
                                            MessageBox.Show("设备不允许使用此工具,请联系管理员开通权限！");
                                        }
                                    }
                                    else
                                    {
                                        mysqlUtil.updateUUID(uuid, "facilitytwo", user.username);
                                        LoginSuccess(mySqlUtil, user);
                                    }
                                }

                            }
                            else
                            {
                                mysqlUtil.updateUUID(uuid, "facility", user.username);
                                LoginSuccess(mySqlUtil, user);
                            }
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
        /// 登录成功，跳转页面
        /// </summary>
        private void LoginSuccess(MysqlUtil mySqlUtil, UserClass user)
        {
            user.logincount++;
            mySqlUtil.updateLoginCount(user.username, user.logincount);
            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            ischeck = (bool)jizhumima.IsChecked;
            Thread thread = new Thread(SaveUserAcc);
            thread.Start();
            this.Close();
        }

        /// <summary>
        /// 登录成功，将用户账号保存到本地
        /// </summary>
        private void SaveUserAcc()
        {
            FileStream fs = null;
            StreamWriter sw = null;
            string encryptuser = StringSecurity.DESEncrypt(usernameep);
            string encryptpw;
           
            fs = new FileStream(userSaveFiler, FileMode.OpenOrCreate);
            sw = new StreamWriter(fs);
            if (ischeck)
            {
                encryptpw = StringSecurity.DESEncrypt(pwp);
            }
            else
            {
                encryptpw = "";
            }
            sw.WriteLine(encryptuser + "&" + encryptpw);
            sw.Close();
            fs.Close();
           
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
