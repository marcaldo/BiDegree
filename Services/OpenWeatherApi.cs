using BiDegree.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace BiDegree.Services
{
    public class OpenWeatherApi : IWeatherApi
    {
        private readonly string _openWeatherApiKey;
        private const string _openWeatherApiAddress = "https://api.openweathermap.org/";

        private readonly HttpClient _httpClient;
        public OpenWeatherApi(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _openWeatherApiKey = configuration.GetValue<string>("OpenWeather:ApiKey");
        }

        public async Task<CurrentWeather> GetCurrentWeatherByCoords(float lat, float lon, string units)
        {
            var url = $"{_openWeatherApiAddress}/data/2.5/weather?lat={lat}&lon={lon}&units={units}&appid={_openWeatherApiKey}";
            var currentWeather = await _httpClient.GetFromJsonAsync<CurrentWeather>(url);

            return currentWeather;
        }
    }
}
