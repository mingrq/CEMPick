﻿using CRMPick.Entity;
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
    /// UpdateUserWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UpdateUserWindow : Window
    {
        private UserClass userclsaa;
        public UpdateUserWindow()
        {
            InitializeComponent();
        }

        public void SetUserEntity(UserClass userclsaa)
        {
            this.userclsaa = userclsaa;
        }


        private void ContentRead(object sender, EventArgs e)
        {
            team.Text = userclsaa.team;
            userpw.Text = userclsaa.userpw;
            userpwagain.Text = userclsaa.userpw;
            username.Content = userclsaa.username;
           string limited= userclsaa.limited;
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
                    default:
                        break;
                }
            }
        }

        private void UpdateUser(object sender, MouseButtonEventArgs e)
        {
            string teamname = team.Text;
            string pw = userpw.Text;
            string pwaga = userpwagain.Text;
            if (teamname.Equals(""))
            {
                MessageBox.Show("请输入使用团队");
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
                user.userpw = pw;
                user.limited = limits;
                user.username = (string)username.Content;
                MysqlUtil mySql = new MysqlUtil();
                bool isupdate = mySql.updateUser(user);
                if (isupdate)
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show("添加失败");
                }
            }
        }
    }
}
