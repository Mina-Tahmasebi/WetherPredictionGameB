using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Service.External_Api
{
    public class WeatherApi
    {
        public static async Task<int> GetCityTemp(string lat, string lng)
        {

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://www.metaweather.com/api/location/search/?lattlong="+ lat+","+lng);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "Get";
            httpWebRequest.Timeout = 5000;
            httpWebRequest.KeepAlive = true;
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                if (string.IsNullOrEmpty(result)) return -1;
                var r = JsonConvert.DeserializeObject<List<WeatherResult>>(result);
                return Convert.ToInt32(r.FirstOrDefault().woeid);
            }
        }
    }
    class WeatherResult
    {
        public string distance { get; set; }
        public string title { get; set; }
        public string location_type { get; set; }
        public string woeid { get; set; }
        public string latt_long { get; set; }
    }
}
