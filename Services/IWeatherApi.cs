using BiDegree.Models;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface IWeatherApi
    {
        Task<CurrentWeather> GetCurrentWeatherByCoords(float lat, float lon, string units);
    }
}
