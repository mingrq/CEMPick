using CRMPick.Entity;
using CRMPick.Utils;
using Newtonsoft.Json;
using Redslide.HttpLib;
using System;
using System.Collections.Generic;
using System.Data;
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
            AddUserWindow addUserWindow = new AddUserWindow(this);
            addUserWindow.ShowDialog();
        }


        private void WinLoaded(object sender, EventArgs e)
        {
            this.Topmost = false;
            Request.Get("https://demo.22com.cn/public/index.php/index/admin/getuserlist",
                         null,
                         result =>
                         {
                             try
                             {
                                 Data<List<UserClass>> data = JsonConvert.DeserializeObject<Data<List<UserClass>>>(result);
                                 if (data.code == 10000)
                                 {
                                     this.Dispatcher.Invoke(new Action(() =>
                                     {
                                         list.ItemsSource = null;
                                         list.ItemsSource = data.result;
                                     }));
                                 }
                                 else
                                 {
                                     string caption = "提示";
                                     string message = data.message;
                                     MessageBox.Show(message, caption);
                                 }
                             }
                             catch (Exception exc)
                             {
                                 MessageBox.Show("用户列表获取失败");
                             }

                         },
                         fail =>
                         {
                             MessageBox.Show("用户列表获取失败");
                         });
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateUser(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var userclass = btn.DataContext as UserClass;
            if (userclass.username.Equals("admin"))
            {
                MessageBox.Show("管理员账号不可修改！！");
            }
            else
            {
                UpdateUserWindow updateUserWindow = new UpdateUserWindow(this);
                updateUserWindow.SetUserEntity(userclass);
                updateUserWindow.Owner = this;
                updateUserWindow.Show();
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var userclass = btn.DataContext as UserClass;
            if (userclass.username.Equals("admin"))
            {
                MessageBox.Show("管理员账号不可删除！！");
            }
            else
            {
                string message = "确认删除该用户吗!";
                string caption = "提示";
                MessageBoxButton buttons = MessageBoxButton.OKCancel;
                // Show message box
                MessageBoxResult result = MessageBox.Show(message, caption, buttons);
                if (result == MessageBoxResult.OK)
                {

                    Request.Post("https://demo.22com.cn/public/index.php/index/admin/deletuser",
                        new
                        {
                            username = userclass.username
                        },
                         delresult =>
                         {
                             try
                             {
                                 Data<string> data = JsonConvert.DeserializeObject<Data<string>>(delresult);
                                 string delresultcaption = "提示";
                                 string delresultmessage = data.message;
                                 if (data.code == 10000)
                                 {
                                     // Show message box
                                     MessageBoxResult msgresult = MessageBox.Show(delresultmessage, delresultcaption);
                                     if (msgresult == MessageBoxResult.OK)
                                     {
                                         this.Dispatcher.Invoke(new Action(() =>
                                         {
                                             List<UserClass> users = (List<UserClass>)list.ItemsSource;
                                             users.Remove(userclass);
                                             list.ItemsSource = null;
                                             list.ItemsSource = users;
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
                                 MessageBox.Show("用户删除失败");
                             }

                         },
                         fail =>
                         {
                             MessageBox.Show("服务器连接失败");
                         });

                }
            }
        }

        /// <summary>
        /// 清空登录设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteFac(object sender, RoutedEventArgs e)
        {
            string message = "确认清空该用户登录设备吗!";
            string caption = "提示";
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            // Show message box
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);
            if (result == MessageBoxResult.OK)
            {
                var btn = sender as Button;
                var userclass = btn.DataContext as UserClass;


                Request.Post("https://demo.22com.cn/public/index.php/index/admin/equipment",
                        new
                        {
                            username = userclass.username
                        },
                         delresult =>
                         {
                             try
                             {
                                 Data<string> data = JsonConvert.DeserializeObject<Data<string>>(delresult);
                                 string delresultcaption = "提示";
                                 string delresultmessage = data.message;
                                 if (data.code == 10000)
                                 {
                                     // Show message box
                                     MessageBoxResult msgresult = MessageBox.Show(delresultmessage, delresultcaption);
                                     if (msgresult == MessageBoxResult.OK)
                                     {
                                         this.Dispatcher.Invoke(new Action(() =>
                                         {
                                             List<UserClass> users = (List<UserClass>)list.ItemsSource;
                                             int i = users.IndexOf(userclass);
                                             users[i].facilitytwo = "";
                                             users[i].facility = "";
                                             list.ItemsSource = null;
                                             list.ItemsSource = users;
                                         }));
                                     }
                                 }
                                 else
                                 {
                                     MessageBox.Show(delresultmessage, delresultcaption);
                                 }
                             }
                             catch (Exception exc)
                             {
                                 MessageBox.Show("设备清空失败");
                             }

                         },
                         fail =>
                         {
                             MessageBox.Show("服务器连接失败");
                         });
            }
        }

        public void addresh(UserClass user)
        {
            List<UserClass> users = (List<UserClass>)list.ItemsSource;
            users.Add(user);
            this.Dispatcher.Invoke(new Action(() =>
            {
                list.ItemsSource = null;
                list.ItemsSource = users;
            }));
        }


        public void updateresh(UserClass user)
        {
            List<UserClass> users = (List<UserClass>)list.ItemsSource;
           for(int i = 0; i < users.Count; i++)
            {
                if(users[i].username == user.username)
                {
                    users[i] = user;
                }
            }
            this.Dispatcher.Invoke(new Action(() =>
            {
                list.ItemsSource = null;
                list.ItemsSource = users;
            }));
        }

        /// <summary>
        /// 密码还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PWBack(object sender, RoutedEventArgs e)
        {
            string message = "确认还原该用户登录密码吗!";
            string caption = "提示";
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            // Show message box
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);
            if (result == MessageBoxResult.OK)
            {
                var btn = sender as Button;
                var userclass = btn.DataContext as UserClass;

                Request.Post("https://demo.22com.cn/public/index.php/index/admin/resetpw",
                        new
                        {
                            username = userclass.username
                        },
                         restresult =>
                         {
                             try
                             {
                                 Data<string> data = JsonConvert.DeserializeObject<Data<string>>(restresult);
                                 string restcaption = "提示";
                                 string restmessage = data.message;
                                 MessageBox.Show(restmessage, restcaption);
                             }
                             catch (Exception exc)
                             {
                                 MessageBox.Show("密码还原失败");
                             }

                         },
                         fail =>
                         {
                             MessageBox.Show("服务器连接失败");
                         });

            }
        }
    }
}
