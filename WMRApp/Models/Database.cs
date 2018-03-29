using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMRApp.Models
{
    class Database
    {
        private MySqlConnection conn;

        public Database()
        {
            conn = new MySqlConnection(@"Server = den1.mysql4.gear.host;Database = jac18; Uid = jac18; Pwd = tp%ipd12");
            conn.Open();
        }

        public int AddUser (User user)
        {
            string sql = "INSERT INTO Users (Name, UserName, Password) VALUES (@Name, @UserName, @Password); " +
                "SELECT LAST_INSERT_ID();";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("Name", user.Name);
                cmd.Parameters.AddWithValue("UserName", user.UserName);
                cmd.Parameters.AddWithValue("Password", user.Password);
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                return id;
            }
        }

        public List<string> AllUserNames()
        {
            List<string> userNames = new List<string>();
            string sql = "SELECT UserName FROM Users;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string userName = (string) reader["UserName"];
                    userNames.Add(userName);
                }
            }
            return userNames;
        }
    }
}
