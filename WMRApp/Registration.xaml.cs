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
        public Registration()
        {
            Global.db = new Database();
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string name = tbName.Text;
            string userName = tbUserName.Text;
            string password = pwbPassword.Password + "";
            string rePassword = pwbRePassword.Password + "";
            if (!password.Equals(rePassword))
            {
                MessageBox.Show("Passwords must be matched");
                return;
            }
            User user = new User() { Name = name, UserName = userName, Password = password };
            int currentUserId = Global.db.AddUser(user);
            if (MessageBox.Show("Registered successfully.\nGo to your account?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                DialogResult = false;
            }
            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void tbUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<string> userNames = Global.db.AllUserNames();
            string userName = tbUserName.Text;
            if (userNames.Contains(userName))
            {
                MessageBox.Show("User name has already existed. Choose a new one.");
                tbUserName.Text = "";
            }
        }
    }
}
