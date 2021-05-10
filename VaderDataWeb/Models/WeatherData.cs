using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VaderDataWeb.Models
{

    public class WeatherData
    {
        public Datum[] data { get; set; }
        public int count { get; set; }
    }

    public class Datum
    {
        public float rh { get; set; }
        public int clouds { get; set; }
        public string city_name { get; set; }
        public float wind_spd { get; set; }
        public float uv { get; set; }    
        public int wind_dir { get; set; }
        public Weather weather { get; set; }
        public string datetime { get; set; }
        public float temp { get; set; }    
        public float app_temp { get; set; }
    }

    public class Weather
    {
        public string icon { get; set; }
        public int code { get; set; }
        public string description { get; set; }
    }


}
