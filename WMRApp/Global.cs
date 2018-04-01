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

        public static string getWeather(string lat, string lng)
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
            string temp = (string)main["temp"];
            string pressure = (string)main["pressure"];
            string humidity = (string)main["humidity"];
            string temp_min = (string)main["temp_min"];
            string temp_max = (string)main["temp_max"];
            result = string.Format("Temperature: {0}, Pressure: {1}, Humidity: {2}, " +
                "Min deg: {3}, Max deg: {4}", temp, pressure, humidity, temp_min, temp_max);
            return result;
        }

        public static string getAddress(string lat, string lng)
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
