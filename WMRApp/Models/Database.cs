using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMRApp.Models
{
    public class Database
    {
        private MySqlConnection conn;

        public Database()
        {
            conn = new MySqlConnection(@"Server = den1.mysql4.gear.host;Database = jac18; Uid = jac18; Pwd = tp%ipd12");
            //conn = new MySqlConnection(@"Server = localhost;Database = trip; Uid = root; Pwd = root");
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

        public User GetUser(string userName, string password)
        {
            User user = null;
            
            string sql = "SELECT Id, Name FROM Users WHERE UserName = @userName and Password = @password;";
            using (MySqlCommand cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("password", password);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = (int) reader["Id"];
                        string name = (string)reader["Name"];
                        user = new User() { Id = id, Name = name, UserName = userName, Password = password };
                    }
                }
            }
            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM Users;", conn);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = (int)r["Id"];
                    string name = (string)r["Name"];
                    string userName = (string)r["UserName"];
                    string password = (string)r["Password"];
                    User user = new User() { Id = id, Name = name, UserName = userName };
                    list.Add(user);
                }
            }
                return list;
        }

        public List<User> GetFoundUsers(string from, string to)
        {
            List<User> list = new List<User>();
            string sql = "SELECT * FROM Users " +
                         "WHERE Id in (" +
                            "SELECT DISTINCT s1.UserId " +
                            "FROM Stops s1 JOIN Stops s2 ON s1.UserId=s2.UserId " +
                            "WHERE s1.Address LIKE \"%" + from + "%\" " +
                            "AND s2.Address LIKE \"%" + to + "%\" " +
                            "AND s1.Id < s2.Id" +
                         ");";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    int id = (int)r["Id"];
                    string name = (string)r["Name"];
                    string userName = (string)r["UserName"];
                    string password = (string)r["Password"];
                    User user = new User() { Id = id, Name = name, UserName = userName };
                    list.Add(user);
                }
            }
            return list;
        }

        public void AddMessageToChats(string content)
        {
            string sql = "INSERT INTO Chats (Content) VALUES (@Content);";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("Content", content);
            cmd.ExecuteNonQuery();
        }

        public List<string> GetAllMessagesFromChats()
        {
            List<string> messages = new List<string>();
            string sql = "SELECT Content FROM Chats;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string message = (string)reader["Content"];
                    messages.Add(message);
                }
            }
            return messages;
        }

        public void AddStop(int userId, double lat, double lng, string address)
        {
            string sql = "INSERT INTO Stops (UserId, Lat, Lng, Address) VALUES (@UserId, @Lat, @Lng, @Address);";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("UserId", userId);
            cmd.Parameters.AddWithValue("Lat", lat);
            cmd.Parameters.AddWithValue("Lng", lng);
            cmd.Parameters.AddWithValue("Address", address);
            cmd.ExecuteNonQuery();
        }

        public List<string> GetAllStopsAddress(int userId)
        {
            List<string> list = new List<string>();
            string sql = "SELECT Address FROM Stops WHERE UserId = @userId;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("userId", userId);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    string address = (string) r["Address"];
                    //Stop stop = new Stop() { Address = address };
                    list.Add(address);
                }
            }
            return list;
        }

        public List<Stop> GetAllStops(int userId)
        {
            List<Stop> list = new List<Stop>();
            string sql = "SELECT Lat, Lng, Address FROM Stops WHERE UserId = @userId;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("userId", userId);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    double lat = (double)r["Lat"];
                    double lng = (double)r["Lng"];
                    string address = (string)r["Address"];
                    Stop stop = new Stop() { Lat = lat, Lng = lng, Address = address };
                    list.Add(stop);
                }
            }
            return list;
        }

        public int GetStopIdSelected(int userId, string address)
        {
            int id = 0;
            string sql = "SELECT Id FROM Stops WHERE UserId = @userId and Address = @address;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("address", address);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                {
                    id = (int)r["Id"];
                }
            }
            return id;
        }

        public void ClearStops(int userId)
        {
            string sql = "DELETE FROM Stops WHERE UserId = @userId;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.ExecuteNonQuery();
        }

        public void DeleteStop(string address, int userId)
        {
            string sql = "DELETE FROM Stops WHERE Address = @address and UserId = @userId";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("address", address);
            cmd.ExecuteNonQuery();
        }

        public int SelectStopId(double lat, double lng)
        {
            int id = 0;
            string sql = "SELECT Id FROM Stops WHERE Lat = @Lat and Lng = @Lng";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("Lat", lat);
            cmd.Parameters.AddWithValue("Lng", lng);
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                {
                    id = (int)r["Id"];
                }
            }
            return id;
        }


        public void UpdateStop (int id, double lat, double lng, string address)
        {
            string sql = "UPDATE Stops SET Lat = @lat, Lng = @lng, Address = @address " +
                "WHERE Id = @id;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("lat", lat);
            cmd.Parameters.AddWithValue("lng", lng);
            cmd.Parameters.AddWithValue("address", address);
            cmd.ExecuteNonQuery();
        }
    }

}
