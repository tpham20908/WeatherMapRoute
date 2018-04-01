using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// Interaction logic for Platform.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int userId;
        private string lat, lng;
        Microsoft.Maps.MapControl.WPF.MapTileLayer tileLayer;

        public MainWindow()
        {
            InitializeComponent();
            // Fires the mouse double click
            MyMap.MouseDoubleClick +=
                new MouseButtonEventHandler(MyMap_MouseDoubleClick);
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (Global.CurrentUser == null)
            {
                Login login = new Login();
                if (login.ShowDialog() != true)
                {
                    Close();
                }
                userId = Global.CurrentUser.Id;
                lblCurrentUser.Content = "User's name: " + Global.CurrentUser.Name;
                refreshUsers();
                refreshChats();
                refreshStops();
            }
        }

        private void refreshUsers()
        {
            lvUsers.ItemsSource = Global.Db.GetAllUsers();
        }

        private void refreshChats()
        {
            lbChats.ItemsSource = Global.Db.GetAllMessagesFromChats();
        }

        private void refreshStops()
        {
            lbStops.ItemsSource = Global.Db.GetAllStopsAddress(userId);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Global.Db.ClearStops(userId);
            refreshStops();
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(MyMap);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = MyMap.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            Pushpin pin = new Pushpin();
            pin.Location = pinLocation;

            // Adds the pushpin to the map.
            MyMap.Children.Add(pin);

            //Gets the map that raised this event
            Map map = (Map)sender;
            //Gets the bounded rectangle for the current frame
            LocationRect bounds = map.BoundingRectangle;
            //Update the current latitude and longitude
            lat = string.Format("{0:F5}", pinLocation.Latitude);
            lng = string.Format("{0:F5}", pinLocation.Longitude);

            tbLocation.Text = Global.getAddress(lat, lng);
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            string msg = Global.CurrentUser.Name + ": " + tbChat.Text;
            Global.Db.AddMessageToChats(msg);
            refreshChats();
            tbChat.Text = "";
        }

        private void btnAddStop_Click(object sender, RoutedEventArgs e)
        {
            string address = tbLocation.Text;
            if (!address.Equals(""))
            {
                Global.Db.AddStop(userId, lat, lng, address);
                refreshStops();
                tbLocation.Text = "";
            }
            else
            {
                MessageBox.Show("Please pick a location to add.");
            }
        }
    }
}
