﻿using CRMPick.Entity;
using CRMPick.Utils;
using System;
using System.Collections.Generic;
using System.Data;
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
            MysqlUtil mysqlUtil = new MysqlUtil();
            List<UserClass> users = mysqlUtil.getUserList();
            list.ItemsSource = users;
            this.Topmost = false;
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
            UpdateUserWindow updateUserWindow = new UpdateUserWindow(this);
            updateUserWindow.SetUserEntity(userclass);
            updateUserWindow.Owner = this;
            updateUserWindow.Show();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteUser(object sender, RoutedEventArgs e)
        {
            string message = "确认删除该用户吗!";
            string caption = "提示";
            MessageBoxButton buttons = MessageBoxButton.OKCancel;
            // Show message box
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);
            if (result==MessageBoxResult.OK)
            {
                var btn = sender as Button;
                var userclass = btn.DataContext as UserClass;
                MysqlUtil mysqlUtil = new MysqlUtil();
                if (mysqlUtil.deleteUser(userclass.username))
                {
                    List<UserClass> users = (List<UserClass>)list.ItemsSource;
                    users.Remove(userclass);
                    list.ItemsSource = null;
                    list.ItemsSource = users;
                }
                else
                {
                    MessageBox.Show("删除失败");
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
                MysqlUtil mysqlUtil = new MysqlUtil();
                if (mysqlUtil.deleteFac(userclass.username))
                {
                    List<UserClass> users = (List<UserClass>)list.ItemsSource;
                    int i = users.IndexOf(userclass);
                    users[i].facilitytwo = "";
                    users[i].facility = "";
                    list.ItemsSource = null;
                    list.ItemsSource = users;
                }
                else
                {
                    MessageBox.Show("清空失败");
                }
            }
        }

        public void resh()
        {
            MysqlUtil mysqlUtil = new MysqlUtil();
            List<UserClass> users = mysqlUtil.getUserList();
            list.ItemsSource = null;
            list.ItemsSource = users;
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
                MysqlUtil mysqlUtil = new MysqlUtil();
                string pw = Encryption.GenerateMD5("111111");
                if (mysqlUtil.pwback(userclass.username,pw))
                {
                    List<UserClass> users = (List<UserClass>)list.ItemsSource;
                    int i = users.IndexOf(userclass);
                    users[i].userpw = pw;
                    MessageBox.Show("密码还原成功!");
                }
                else
                {
                    MessageBox.Show("还原失败");
                }
            }
        }
    }
}
