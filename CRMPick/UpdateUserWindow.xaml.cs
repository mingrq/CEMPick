﻿using CRMPick.Entity;
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
    /// UpdateUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateUserWindow : Window
    {
        private UserClass userclsaa;
        UserControlWindow userControlWindow;
        public UpdateUserWindow(UserControlWindow userControlWindow)
        {
            InitializeComponent();
            this.userControlWindow = userControlWindow;

        }

        public void SetUserEntity(UserClass userclsaa)
        {
            this.userclsaa = userclsaa;
        }


        private void ContentRead(object sender, EventArgs e)
        {

            team.Text = userclsaa.team;
            username.Content = userclsaa.username;
            tiaoruresourcecount.Text = userclsaa.tiaoruresourcecount.ToString();
            gatherresourcecount.Text = userclsaa.gatherresourcecount.ToString();
            string limited = userclsaa.limited;
            string[] limits = limited.Split(',');
            for (int i = 0; i < limits.Length; i++)
            {
                int lim = int.Parse(limits[i]);
                switch (lim)
                {
                    case 1:
                        crmcb.IsChecked = true;
                        break;
                    case 2:
                        pccb.IsChecked = true;
                        break;
                    case 3:
                        ptcb.IsChecked = true;
                        break;
                    case 4:
                        usercb.IsChecked = true;
                        break;
                    case 5:
                        quanliang.IsChecked = true;
                        break;
                    default:
                        break;
                }
            }

        }

        private void UpdateUser(object sender, MouseButtonEventArgs e)
        {
            string teamname = team.Text;
            string tiaorucount = tiaoruresourcecount.Text.Trim().Length > 9 ? "999999999" : tiaoruresourcecount.Text.Trim();
            string gathercount = gatherresourcecount.Text.Trim().Length > 9 ? "999999999" : gatherresourcecount.Text.Trim();
            if (teamname.Equals(""))
            {
                MessageBox.Show("请输入使用团队");
            }
            else
            {
                string limits = "1";
                limits += pccb.IsChecked == true ? ",2" : "";
                limits += ptcb.IsChecked == true ? ",3" : "";
                limits += usercb.IsChecked == true ? ",4" : "";
                limits += quanliang.IsChecked == true ? ",5" : "";
                UserClass user = new UserClass();
                user.team = teamname;
                user.limited = limits;
                user.username = (string)username.Content;
                user.tiaoruresourcecount = long.Parse(tiaorucount) > 999999999 ? 999999999 : int.Parse(tiaorucount);
                user.gatherresourcecount = long.Parse(gathercount) > 999999999 ? 999999999 : int.Parse(gathercount);


                Request.Post("https://demo.22com.cn/public/index.php/index/admin/updateuser",
                        new
                        {
                            team = teamname,
                            username = (string)username.Content,
                            limited = limits,
                            tiaoruresourcecount = long.Parse(tiaorucount) > 999999999 ? 999999999 : int.Parse(tiaorucount),
                            gatherresourcecount = long.Parse(gathercount) > 999999999 ? 999999999 : int.Parse(gathercount)
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
                                             userControlWindow.updateresh(user);
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
