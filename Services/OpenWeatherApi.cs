using BiDegree.Models;
using BiDegree.Shared;
using Blazored.LocalStorage;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public class OpenWeatherApi : IWeatherApi
    {
        private readonly string _openWeatherApiKey;
        private const string _openWeatherApiAddress = "https://api.openweathermap.org/";

        private readonly HttpClient _httpClient;
        public OpenWeatherApi(HttpClient httpClient, ISyncLocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _openWeatherApiKey = localStorageService.GetItem<string>(Constants.KeyApiKey);
        }

        public async Task<CurrentWeather> GetCurrentWeatherByCoords(float lat, float lon, string units)
        {
            var url = $"{_openWeatherApiAddress}/data/2.5/weather?lat={lat}&lon={lon}&units={units}&appid={_openWeatherApiKey}";
            return await GetCurrentWeather(url, units);
        }

        public async Task<CurrentWeather> GetCurrentWeatherByCity(string city, string units)
        {
            var url = $"{_openWeatherApiAddress}/data/2.5/weather?q={city}&units={units}&appid={_openWeatherApiKey}";
            return await GetCurrentWeather(url, units);
        }

        public async Task<CurrentWeather> GetCurrentWeather(string url, string units)
        {
            var currentWeather = await _httpClient.GetFromJsonAsync<CurrentWeather>(url);
            return currentWeather;
        }
    }
}
