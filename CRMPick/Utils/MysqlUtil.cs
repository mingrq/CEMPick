using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            UserClass user=null;
            MySqlConnection conn = MySql();
            string sql = "select * from crm_user where username=\'"+ username+"\'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                user = new UserClass();
                user.username = reader.GetString("username");
                user.userpw = reader.GetString("userpw");
                user.facility = reader.GetString("facility");
                user.limited = reader.GetInt16("limited");
                user.token = reader.GetString("token");
            }
            conn.Close();
            return user;
        }
    }
}
