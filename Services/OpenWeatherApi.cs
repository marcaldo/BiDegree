using BiDegree.Models.OpenWeather.WeatherModels;
using BiDegree.Models.OpenWeather.AirPollutionModels;
using BiDegree.Shared;
using Blazored.LocalStorage;
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
            _openWeatherApiKey = localStorageService.GetItem<string>(Constants.KeyName_WeatherApiKey);
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

        public async Task<AirPollution> GetAirPollution(float lat, float lon)
        {
                var url = $"{_openWeatherApiAddress}/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_openWeatherApiKey}";
                var airPollution = await _httpClient.GetFromJsonAsync<AirPollution>(url);
                return airPollution;
        }

        public async Task<CurrentWeather> GetCurrentWeather(string url, string units = null)
        {
            var currentWeather = await _httpClient.GetFromJsonAsync<CurrentWeather>(url);
            return currentWeather;
        }
    }
}
