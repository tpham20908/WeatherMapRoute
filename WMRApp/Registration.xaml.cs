using MySql.Data.MySqlClient;
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
using WMRApp.Models;

namespace WMRApp
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public User user;
        public Registration()
        {
            try
            {
                Global.Db = new Database();
                InitializeComponent();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.StackTrace);
                MessageBox.Show("Error opening database connection: " + e.Message);
                Environment.Exit(1);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            List<string> userNames = Global.Db.AllUserNames();
            string name = tbName.Text;
            string userName = tbUserName.Text;
            if (userNames.Contains(userName))
            {
                MessageBox.Show("User name has already existed. Choose a new one.");
                tbUserName.Text = "";
                return;
            }
            string password = pwbPassword.Password;
            string rePassword = pwbRePassword.Password;
            if (!password.Equals(rePassword))
            {
                MessageBox.Show("Passwords must be matched");
                return;
            }
            User newUser = new User() { Name = name, UserName = userName, Password = password };
            int currentUserId = Global.Db.AddUser(newUser);
            user = Global.Db.GetUser(userName, password);
            if (MessageBox.Show("Registered successfully. Go to your account?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                DialogResult = false;
            }
            else
            {
                DialogResult = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
