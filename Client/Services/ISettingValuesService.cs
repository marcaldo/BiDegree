using System.Threading.Tasks;

namespace BiDegree.Services
{
    public interface ISettingValuesService
    {
        Task<(int imageCount, int duration)> GetWeatherExtendedValuesAsync();
        void SetWeatherExtendedValues(int waitSeconds, int durationSeconds);
    }
}
