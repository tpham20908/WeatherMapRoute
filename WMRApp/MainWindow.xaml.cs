using Microsoft.Maps.MapControl.WPF;
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
    /// Interaction logic for Platform.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            //MyMap.ViewChangeOnFrame += new EventHandler<MapEventArgs>(MyMap_ViewChangeOnFrame);
            // Fires when the left mouse button is depressed

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
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

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
            tbLocation.Text = "";
            tbLocation.Text = String.Format("Latitude: {0:F5}\nLongitude: {1:F5}", bounds.North, bounds.West);
        }
    }
}
