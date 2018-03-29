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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            Registration r = new Registration();
            if (r.ShowDialog() == true)
            {
                this.Close();
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string userName = tbUserName.Text;
            string password = pwbPassword.Password;
            User user = Global.db.GetUser(userName, password);
            if (user != null)
            {
                Platform p = new Platform(user);
                if (p.ShowDialog() == true)
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("User does not exist. Try again!");
                tbUserName.Text = "";
                pwbPassword.Password = "";
            }
        }
    }
}
