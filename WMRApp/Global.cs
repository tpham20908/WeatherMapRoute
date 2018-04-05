using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WMRApp.Models;

namespace WMRApp
{
    public class Global
    {
        public static Database Db;
        public static User CurrentUser;
        public static string mapKey = "AhOYVsCHeLfCM2LttVNiVAK6mUGtJmjRlevk_2qjuzV9J-gNrsj6z6MD5XREJN1h";
        public static string weatherKey = "a2cc469ef8f6b9c326aeb4f98c875cdd"; //appid on https://openweathermap.org

        private static double KtoC(double k)
        {
            return k - 273.15;
        }

        public static string getWeather(double lat, double lng)
        {
            string url = "http://api.openweathermap.org/data/2.5/weather?lat="
                + lat + "&lon=" + lng + "&appid=" + weatherKey;

            var request = WebRequest.Create(url);
            string result;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            // parsing the result
            JObject joText = JObject.Parse(result);
            JObject main = (JObject)joText["main"];
            string tempStr = (string)main["temp"];
            double temp = KtoC(double.Parse(tempStr));
            string pressure = (string)main["pressure"];
            string humidity = (string)main["humidity"];
            string temp_minStr = (string)main["temp_min"];
            double temp_min = KtoC(double.Parse(temp_minStr));
            string temp_maxStr = (string)main["temp_max"];
            double temp_max = KtoC(double.Parse(temp_maxStr));
            result = string.Format("Temperature: {0:F0}°C, Pressure: {1}hPa, Humidity: {2}%, " +
                "\nMin deg: {3:F0}°C, Max deg: {4:F0}°C", temp, pressure, humidity, temp_min, temp_max);
            return result;
        }

        public static string getAddress(double lat, double lng)
        {
            string url = "http://dev.virtualearth.net/REST/v1/Locations/" 
                          + lat + "," + lng + "?&key=" + mapKey;
            var request = WebRequest.Create(url);
            string result;
            var response = (HttpWebResponse)request.GetResponse();

            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }
            // parsing the result
            JObject joText = JObject.Parse(result);
            JArray resourceSets = (JArray)joText["resourceSets"];
            JArray resources = (JArray)resourceSets[0]["resources"];
            string name = (string)resources[0]["name"];
            return name;
        }
    }
}
