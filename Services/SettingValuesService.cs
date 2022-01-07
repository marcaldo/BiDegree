using BiDegree.Shared;
using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace BiDegree.Services
{
    // TODO: Pass all the settings handle here
    public class SettingValuesService : ISettingValuesService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ISyncLocalStorageService _syncLocalStorage;

        public SettingValuesService(ILocalStorageService localStorage, ISyncLocalStorageService syncLocalStorage)
        {
            _localStorage = localStorage;
            _syncLocalStorage = syncLocalStorage;

        }
        public async Task<(int imageCount, int duration)> GetWeatherExtendedValuesAsync()
        {
            var weatherExtendedValues = await _localStorage.GetItemAsStringAsync(Constants.KeyName_WeatherExtended);
            if (weatherExtendedValues != null && weatherExtendedValues.Contains("."))
            {
                var storedValues = weatherExtendedValues.Split('.');

                _ = int.TryParse(storedValues[0], out int imageCountToShowWeather);
                _ = int.TryParse(storedValues[1], out int duration);

                if (imageCountToShowWeather > 0 && duration > 0)
                {
                    const int ms = 1000;
                    return (imageCountToShowWeather * ms, duration * ms);
                }
            }

            return (0, 0);
        }

        public void SetWeatherExtendedValues(int waitSeconds, int durationSeconds)
        {
            string values = $"{waitSeconds}.{durationSeconds}";
            _syncLocalStorage.SetItemAsString(Constants.KeyName_WeatherExtended, values);
        }
    }
}
