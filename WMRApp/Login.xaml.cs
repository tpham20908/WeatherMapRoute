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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMRApp.Models;

namespace WMRApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Login : Window
    {
        
        public Login()
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

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            Registration r = new Registration();
            if (r.ShowDialog() == true)
            {
                Global.CurrentUser = null;
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = tbUserName.Text;
            string password = pwbPassword.Password;
            User user = Global.Db.GetUser(userName, password);
            if (user != null)
            {
                DialogResult = true;
                Global.CurrentUser = user;
            }
            else
            {
                MessageBox.Show("User does not exist. Try again!");
                tbUserName.Text = "";
                pwbPassword.Password = "";
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
