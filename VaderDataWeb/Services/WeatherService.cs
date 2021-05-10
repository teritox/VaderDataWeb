using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VaderDataWeb.Models;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;
using VaderDataWeb;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace VaderDataWeb.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        public WeatherService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IEnumerable<Measurement> GetValuesOfToday(string city)
        {
            var client = new HttpClient();
            var Dal = new DAL(_configuration);

            var collection = Dal.MeasurementCollection().Find(new BsonDocument()).ToList();

            var valuesOfTheDay = collection.Where(d => d.Date.Date == DateTime.Now.Date && d.City == city);

            return valuesOfTheDay.ToList();

        }
        public async Task<IEnumerable<Measurement>> GetCurrentWeatherAllAsync(string[] cities)
        {
            List<Measurement> weatherList = new List<Measurement>();
            var client = new HttpClient();
            var collection = new DAL(_configuration);


            foreach (var city in cities)
            {

                string connectionString = _configuration.GetConnectionString("WeatherIO");
                Task<string> getWeatherStringTask = client.GetStringAsync($"{connectionString + city}");
                try
                {
                    string weatherString = await getWeatherStringTask;
                    var weatherData = JsonSerializer.Deserialize<Models.WeatherData>(weatherString);

                    var measurement = new Models.Measurement
                    {
                        Id = Guid.NewGuid(),
                        City = city,
                        Date = DateTime.Now,
                        Values = new Models.Values
                        {
                            RelativeHumidity = weatherData.data[0].rh,
                            Temp = weatherData.data[0].temp,
                            WindSpeed = weatherData.data[0].wind_spd,
                            WindDir = weatherData.data[0].wind_dir,
                            Uv = weatherData.data[0].uv,
                            CloudCoverage = weatherData.data[0].clouds,
                            App_Temp = weatherData.data[0].app_temp,
                            Description = weatherData.data[0].weather.description
                        }
                    };

                    await collection.MeasurementCollection().InsertOneAsync(measurement);

                    weatherList.Add(measurement);

                }
                catch
                {

                }

            }

            return weatherList;

        }

        public string GetSpring(string city)
        {
            var client = new HttpClient();
            var Dal = new DAL(_configuration);

            var collection = Dal.MeasurementCollection().Find(new BsonDocument()).ToList();

            var springCollection = collection
                .Where(d => d.City == city)
                .GroupBy(d => d.Date.Date)
                .Select(d => new { Date = d.Key, Avg = d.Average(a => a.Values.Temp) })
                .OrderBy(d => d.Date)
                .ToList();


            int daysInARow = 0;


            for (int i = 0; i < springCollection.Count - 1; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (springCollection[i].Date.Month >= 2 && springCollection[i].Date.Day >= 15)
                    {
                        if (Math.Round((decimal)springCollection[i + j].Avg, 1) > 0 && Math.Round((decimal)springCollection[i + j].Avg, 1) < 10)
                        {
                            daysInARow++;
                        }
                        else
                        {
                            daysInARow = 0;
                            break;
                        }

                        if (daysInARow == 7)
                        {
                            string spring = springCollection[i].Date.ToString();
                            return spring;
                        }
                    }
                }
                if (daysInARow == 7)
                {
                    break;
                }
            }
            return "Ingen datum hittades";
        }

        public string GetSummer(string city)
        {
            var client = new HttpClient();
            var Dal = new DAL(_configuration);

            var collection = Dal.MeasurementCollection().Find(new BsonDocument()).ToList();

            var summerCollection = collection
                .Where(d => d.City == city)
                .GroupBy(d => d.Date.Date)
                .Select(d => new { Date = d.Key, Avg = d.Average(a => a.Values.Temp) })
                .OrderBy(d => d.Date)
                .ToList();


            int daysInARow = 0;


            for (int i = 0; i < summerCollection.Count - 1; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (summerCollection[i].Date.Month >= 2 && summerCollection[i].Date.Day >= 15)
                    {
                        if (Math.Round((decimal)summerCollection[i + j].Avg, 1) >= 10)
                        {
                            daysInARow++;
                        }
                        else
                        {
                            daysInARow = 0;
                            break;
                        }

                        if (daysInARow == 5)
                        {
                            string spring = summerCollection[i].Date.ToString();
                            return spring;
                        }
                    }

                    if (daysInARow == 5)
                    {
                        break;
                    }
                }
            }
            return "Ingen datum hittades";
        }
    }

}
