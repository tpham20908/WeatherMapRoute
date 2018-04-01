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
