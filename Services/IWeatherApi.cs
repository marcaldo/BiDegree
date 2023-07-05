using BiDegree.Models.OpenWeather.WeatherModels;
using BiDegree.Models.OpenWeather.AirPollutionModels;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IWeatherApi
    {
        Task<CurrentWeather> GetCurrentWeatherByCoords(float lat, float lon, string units);
        Task<CurrentWeather> GetCurrentWeatherByCity(string city, string units);
        Task<CurrentWeather> GetCurrentWeather(string url, string units);
        Task<AirPollution> GetAirPollution(float lat, float lon);
    }
}
