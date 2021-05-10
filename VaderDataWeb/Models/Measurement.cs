using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace VaderDataWeb.Models
{
    public class Measurement
    {
        public Guid Id { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }
        public Values Values { get; set; }
    }
    public class Values
    {
        public float RelativeHumidity { get; set; }
        public int CloudCoverage { get; set; }
        public double WindSpeed { get; set; }
        public int WindDir { get; set; }
        public float Uv { get; set; }
        public Weather weather { get; set; }
        public float Temp { get; set; }
        public float App_Temp { get; set; }
        public string Description { get; set; }
    }
}



