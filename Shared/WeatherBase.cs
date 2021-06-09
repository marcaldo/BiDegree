using BiDegree.Models;
using BiDegree.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;

namespace BiDegree.Shared
{
    public class WeatherBase : ComponentBase
    {
        [Inject] HttpClient Http { get; set; }
        [Inject] IWeatherApi OpenWeather { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }

        protected static Func<float, float, Task> getWeatherFunc;
        protected string errorMessage = null;
        protected bool unauthorized = false;
        protected bool useCity = false;
        protected string city = null;
        protected CurrentWeather currentWeather { get; set; }
        protected UnitsType units;


        private void Timer_Elapsed(object _, ElapsedEventArgs e)
        {
            RefreshHandler().Wait();
        }

        public async Task RefreshHandler()
        {
            await GetCurrentWeather(currentWeather.coord.lat, currentWeather.coord.lon);
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            getWeatherFunc = GetWeather;
        }

        protected override async Task OnInitializedAsync()
        {
            useCity = await LocalStorage.GetItemAsync<bool>(Constants.KeyUseCity);
            city = await LocalStorage.GetItemAsync<string>(Constants.KeyCity);

            var storedRefreshMinutes = await LocalStorage.GetItemAsync<int?>(Constants.RefreshTime);
            int refreshMinutes = storedRefreshMinutes is null
                                    ? Constants.DefaultRefresh
                                    : Convert.ToInt32(storedRefreshMinutes);

            var unitsConfig = await LocalStorage.GetItemAsync<UnitsType?>(Constants.KeyUnits);
            units = unitsConfig ?? UnitsType.Metric;

            await GetGeoLocation();

            if (refreshMinutes > 0)
            {
                Timer timer = new();
                timer.Interval = 1000 * 60 * refreshMinutes;
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
            }
        }

        public async Task GetGeoLocation()
        {
            await JS.InvokeAsync<string>("getCurrentLocation");
        }

        [JSInvokable]
        public static void GetWeatherCaller(float lat, float lon)
        {
            getWeatherFunc.Invoke(lat, lon);
        }

        private async Task GetWeather(float lat, float lon)
        {
            bool validLatLon = lat + lon != 0;
            bool canUseCity = useCity && !string.IsNullOrEmpty(city);

            unauthorized = !validLatLon && !canUseCity;

            if (!unauthorized)
            {
                await GetCurrentWeather(lat, lon);
            }

            StateHasChanged();
        }

        private async Task GetCurrentWeather(float lat, float lon)
        {
            try
            {
                if (useCity)
                {
                    currentWeather = await OpenWeather.GetCurrentWeatherByCity(city, units.ToString().ToLower());
                }
                else
                {
                    currentWeather = await OpenWeather.GetCurrentWeatherByCoords(lat, lon, units.ToString().ToLower());
                }
            }
            catch (HttpRequestException ex)
            {
                unauthorized = ex.StatusCode == HttpStatusCode.Unauthorized;
                errorMessage = ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }
    }
}
