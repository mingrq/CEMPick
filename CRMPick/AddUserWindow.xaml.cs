using CRMPick.Entity;
using CRMPick.Utils;
using Newtonsoft.Json;
using Redslide.HttpLib;
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
       
        UserControlWindow userControlWindow;
        public AddUserWindow(UserControlWindow userControlWindow)
        {
            InitializeComponent();
            this.userControlWindow = userControlWindow;
        }

        private void AddUser(object sender, MouseButtonEventArgs e)
        {
         
                string teamname = team.Text.Trim();
                string userna = username.Text.Trim();
                string tiaorucount = tiaoruresourcecount.Text.Trim().Length > 9 ? "999999999" : tiaoruresourcecount.Text.Trim();
                string gathercount = gatherresourcecount.Text.Trim().Length > 9 ? "999999999" : gatherresourcecount.Text.Trim();

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
                    user.tiaoruresourcecount = long.Parse(tiaorucount) > 999999999 ? 999999999 : int.Parse(tiaorucount);
                    user.gatherresourcecount = long.Parse(gathercount) > 999999999 ? 999999999 : int.Parse(gathercount);
                    Request.Post("https://demo.22com.cn/public/index.php/index/admin/adduser",
                    new
                    {
                        team = teamname,
                        username = userna,
                        limited = limits,
                        tiaoruresourcecount = long.Parse(tiaorucount) > 999999999 ? 999999999 : int.Parse(tiaorucount),
                        gatherresourcecount = long.Parse(gathercount) > 999999999 ? 999999999 : int.Parse(gathercount)
                    },
                    addresult =>
                    {
                        Console.WriteLine(addresult);
                        try
                        {
                            Data<string> data = JsonConvert.DeserializeObject<Data<string>>(addresult);
                            string addresultcaption = "提示";
                            string addresultmessage = data.message;
                            if (data.code == 10000)
                            {
                                // Show message box
                                MessageBoxResult msgresult = MessageBox.Show(addresultmessage, addresultcaption);
                                if (msgresult == MessageBoxResult.OK)
                                {
                                    this.Dispatcher.Invoke(new Action(() =>
                                    {
                                        userControlWindow.addresh(user);
                                        this.Close();
                                    }));
                                }
                            }
                            else
                            {
                                MessageBox.Show(addresultmessage, addresultcaption);
                            }
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("添加用户失败");
                        }

                    },
                     fail =>
                     {
                         Console.WriteLine(fail.ToString());
                         MessageBox.Show("服务器连接失败");
                     });
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
