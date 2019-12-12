using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CRMPick.Entity;
using MySql.Data.MySqlClient;

namespace CRMPick.Utils
{
    class MysqlUtil
    {
        string connetStr = "server=demo.22com.cn;port=3306;user=demo_22com_cn;password=ndQmFB2tzhAjeGDB; database=demo_22com_cn;";
        public MySqlConnection MySql()
        {
            MySqlConnection conn = new MySqlConnection(connetStr);
            try
            {
                conn.Open();
                Console.WriteLine("已经建立连接");
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server, Contact administrator");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }
            }
            return conn;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public UserClass getUser(string username)
        {
            UserClass user = null;
            MySqlConnection conn = MySql();
            string sql = "select * from crm_user where username=\'" + username + "\'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user = new UserClass();
                user.username = reader.GetString("username");
                user.userpw = reader.GetString("userpw");
                user.facility = reader.GetString("facility");
                user.facilitytwo = reader.GetString("facilitytwo");
                user.limited = reader.GetString("limited");
                user.logincount = reader.GetInt16("logincount");
            }
            conn.Close();
            return user;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool addUser(UserClass user)
        {
            MySqlConnection conn = MySql();
            string sql = "INSERT INTO crm_user(`username`, `userpw`,  `limited`, `team`) VALUES ('" + user.username + "', '" + user.userpw + "', '" + user.limited + "',  '" + user.team + "')";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int tian = cmd.ExecuteNonQuery();
            conn.Close();
            if (tian > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal void getUserNameRepeat(TextBox username)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool updateUser(UserClass user)
        {
            MySqlConnection conn = MySql();
            string sql = "UPDATE crm_user SET `team` = '" + user.team + "', `userpw` = '" + user.userpw + "', `limited` = '" + user.limited + "' WHERE `username` = '" + user.username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int tian = cmd.ExecuteNonQuery();
            conn.Close();
            if (tian > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 修改uuid
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool updateUUID(string uuid, string facility, string username)
        {
            MySqlConnection conn = MySql();
            string sql = "UPDATE crm_user SET `" + facility + "` = '" + uuid + "' WHERE `username` = '" + username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int update = cmd.ExecuteNonQuery();
            conn.Close();
            if (update > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="username"></param>
        public bool getUserNameRepeat(string username)
        {
            MySqlConnection conn = MySql();
            string sql = "select * from crm_user where `username` = '" + username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            return reader.HasRows;
        }

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns></returns>
        public List<UserClass> getUserList()
        {
            List<UserClass> users = new List<UserClass>();
            MySqlConnection conn = MySql();
            string sql = "select * from crm_user";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                UserClass user = new UserClass();
                user.team = reader.GetString("team");
                user.username = reader.GetString("username");
                user.userpw = reader.GetString("userpw");
                user.facility = reader.GetString("facility");
                user.facilitytwo = reader.GetString("facilitytwo");
                user.limited = reader.GetString("limited");
                user.logincount = reader.GetInt16("logincount");
                users.Add(user);
            }
            return users;
        }

        /// <summary>
        /// 清空设备
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool deleteFac(string username)
        {
            MySqlConnection conn = MySql();
            string sql = "UPDATE crm_user SET `facility` = '', `facilitytwo` = '' WHERE `username` = '" + username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int update = cmd.ExecuteNonQuery();
            conn.Close();
            if (update > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool deleteUser(string username)
        {
            MySqlConnection conn = MySql();
            string sql = "DELETE FROM crm_user WHERE `username` = '" + username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int update = cmd.ExecuteNonQuery();
            conn.Close();
            if (update > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 修改登录次数
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool updateLoginCount(string username,int count)
        {
            MySqlConnection conn = MySql();
            string sql = "UPDATE crm_user SET `logincount` = '" + count + "' WHERE `username` = '" + username + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            int update = cmd.ExecuteNonQuery();
            conn.Close();
            if (update > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
