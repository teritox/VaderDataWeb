using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaderDataWeb.Services;

namespace VaderDataWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IWeatherService _weather;

        public string[] cities = { "Kiruna", "Sundsvall", "Stockholm", "Jönköping", "Göteborg", "Malmö" };

        [BindProperty(SupportsGet = true)]
        public string City { get; set; }

        public IndexModel(IWeatherService weather)
        {
            _weather = weather;
        }

        public void OnGet()
        {



        }
    }
}
