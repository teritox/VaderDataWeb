using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaderDataWeb.Models;

namespace VaderDataWeb.Services
{
    public interface IWeatherService
    {
        string GetSpring(string city);
        string GetSummer(string city);
        IEnumerable<Measurement> GetValuesOfToday(string city);
        Task<IEnumerable<Measurement>> GetCurrentWeatherAllAsync(string[] cities);
    }
}
