using BiDegree.Models;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IWeatherApi
    {
        Task<CurrentWeather> GetCurrentWeatherByCoords(float lat, float lon, string units);
        Task<CurrentWeather> GetCurrentWeatherByCity(string city, string units);
        Task<CurrentWeather> GetCurrentWeather(string url, string units);
    }
}
