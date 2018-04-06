using Microsoft.Maps.MapControl.WPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public double lat, lng;

        public MainWindow()
        {
            InitializeComponent();
            // Fires the mouse double click
            MyMap.MouseDoubleClick += new MouseButtonEventHandler(MyMap_MouseDoubleClick);
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
                else
                {
                    userId = Global.CurrentUser.Id;
                    lblCurrentUser.Content = "User's name: " + Global.CurrentUser.Name;
                    refreshUsers();
                    refreshChats();
                    refreshStops();
                    refreshPushpins();
                }
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

        public async void TraceRoot(List<Stop> stopList)
        {
            List<ResourceSet> resourceSet = new List<ResourceSet>();
            Resource resource;
            List<ItineraryItem> items = new List<ItineraryItem>();
            List<Location> loc = new List<Location>();

            if (stopList.Count < 2)
            {
                return;
            }

            for (int i = 0; i < stopList.Count - 2; i++)
            {
                Console.WriteLine("A stop called.");
                string point1 = stopList[i].Lat + "," + stopList[i].Lng;
                string point2 = stopList[i + 1].Lat + "," + stopList[i + 1].Lng;
                try
                {
                    string reqURL = "http://dev.virtualearth.net/REST/V1/Routes?wp.0=" + point1 + "&wp.1=" + point2 + "&key=" + Global.mapKey;
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(reqURL);
                    var jsonResponse = await response.Content.ReadAsStringAsync();

                    // Parsing JSON Response
                    var rootObject = JsonConvert.DeserializeObject<RootObject>(jsonResponse);

                    foreach (ResourceSet set in rootObject.resourceSets)
                    {
                        resourceSet.Add(set);
                    }

                    loc.Clear();

                    resource = resourceSet[0].resources[0];

                    items = resource.routeLegs[0].itineraryItems;

                    // Colleting location points to draw route got in response. 

                    foreach (ItineraryItem item in items)
                    {
                        loc.Add(new Location() { Latitude = item.maneuverPoint.coordinates[0], Longitude = item.maneuverPoint.coordinates[1] });
                    }

                    // Declaring Object of MapPolyline to Draw Route

                    MapPolyline line = new MapPolyline();

                    // Defining color to Polyline that is Red
                    line.Stroke = new SolidColorBrush(Colors.Red);
                    line.Width = 5;
                    
                    // Giving Collection of location points to Map Polyline     
                    foreach (Location l in loc)
                    {
                        line.Locations.Add(l);
                    }

                    // Defining Map Shape layer Object to add Polyline shape to it. 
                    //MapShapeLayer shapeLayer = new MapShapeLayer();
                    MapLayer mapLayer = new MapLayer();

                    // Adding line to Shape Layer 
                    mapLayer.Children.Add(line);

                    // Adding Shape Layer to Map
                    MyMap.Children.Add(mapLayer);
                    

                    // Calculating Mid between both location to set center of Map
                    int mid;

                    if (loc.Count % 2 == 0)
                    {
                        mid = loc.Count / 2;
                    }
                    else
                    {
                        mid = (loc.Count + 1) / 2;
                    }

                    MyMap.Center = loc[mid];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        public void refreshPushpins()
        {
            List<Stop> stopList = Global.Db.GetAllCoordinates(userId);
            TraceRoot(stopList);
            foreach (Stop stop in stopList)
            {
                double lat = stop.Lat;
                double lng = stop.Lng;
                var pin = new DraggablePin(MyMap, DraggablePinDroppedHanlder);
                pin.Location = new Location(lat, lng);
                MyMap.Children.Add(pin);

                string weather = Global.getWeather(lat, lng);
                ToolTipService.SetToolTip(pin, weather);
            }
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

        public void DraggablePinDroppedHanlder(DraggablePin pin)
        {
            //Console.WriteLine("Pin dropped");
            var pinLocation = pin.Location;
            //Update the current latitude and longitude
            lat = pinLocation.Latitude;
            lng = pinLocation.Longitude;
            tbLocation.Text = Global.getAddress(lat, lng);
        }

        private void MyMap_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            // Disables the default mouse double-click action.
            e.Handled = true;

            // Determin the location to place the pushpin at on the map.

            //Get the mouse click coordinates
            Point mousePosition = e.GetPosition(MyMap);
            //Convert the mouse coordinates to a locatoin on the map
            Location pinLocation = MyMap.ViewportPointToLocation(mousePosition);

            // The pushpin to add to the map.
            DraggablePin pin = new DraggablePin(MyMap, DraggablePinDroppedHanlder);
            pin.Location = pinLocation;

            // Adds the pushpin to the map.
            MyMap.Children.Add(pin);

            //Update the current latitude and longitude
            lat = pinLocation.Latitude;
            lng = pinLocation.Longitude;
            tbLocation.Text = Global.getAddress(lat, lng);

            string weather = Global.getWeather(lat, lng);
            ToolTipService.SetToolTip(pin, weather);
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
