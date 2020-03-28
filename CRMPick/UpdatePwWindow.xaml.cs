using CRMPick.Entity;
using CRMPick.Utils;
using Newtonsoft.Json;
using Redslide.HttpLib;
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
                Request.Post("https://demo.22com.cn/public/index.php/index/admin/alterpw",
                        new
                        {
                            username = userclsaa.username,
                            pw = Encryption.GenerateMD5(userpwa)
                        },
                         result =>
                         {
                             try
                             {
                                 Data<string> data = JsonConvert.DeserializeObject<Data<string>>(result);
                                 string caption = "提示";
                                 string message = data.message;
                                 if (data.code == 10000)
                                 {
                                  // Show message box
                                  MessageBoxResult msgresult = MessageBox.Show(message, caption);
                                     if (msgresult == MessageBoxResult.OK)
                                     {
                                         this.Dispatcher.Invoke(new Action(() =>
                                         {
                                             this.Close();
                                         }));
                                     }
                                 }
                                 else
                                 {
                                     MessageBox.Show(message, caption);
                                 }
                             }
                             catch (Exception exc)
                             {
                                 MessageBox.Show("密码修改失败");
                             }

                         },
                         fail =>
                         {
                             MessageBox.Show("服务器连接失败");
                         });
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
