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
using Redslide.HttpLib;
using Newtonsoft.Json;

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
                string uuid = UUIDClass.GetUUID();
                Object users = new
                {
                    username = usernamee,
                    pw = pw,
                    uuid = uuid
                };
                Request.Post("https://demo.22com.cn/public/index.php/index/login/login", users,
                    result =>
                    {
                    try
                    {
                        LoginClass login = JsonConvert.DeserializeObject<LoginClass>(result);
                        if (login.code == 10000)
                        {
                            this.Dispatcher.Invoke(new Action(() => {
                                MainWindow mainWindow = new MainWindow(login.result);
                                mainWindow.Show();
                                ischeck = (bool)jizhumima.IsChecked;
                                Thread thread = new Thread(SaveUserAcc);
                                thread.Start();
                                this.Close();
                            }));
                               
                            }
                            else
                            {
                                MessageBox.Show(login.message);
                            }
                        }
                        catch(Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                            MessageBox.Show("网络连接失败");
                        }
                    },
                    fail =>
                    {
                        MessageBox.Show("网络连接失败");
                    });
            }
        }


        /// <summary>
        /// 登录成功，将用户账号保存到本地
        /// </summary>
        private void SaveUserAcc()
        {
            try
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
            catch
            {

            }
        }

      
    }
}
