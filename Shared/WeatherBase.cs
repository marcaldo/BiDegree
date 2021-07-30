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
        [Inject] IWeatherApi OpenWeather { get; set; }
        [Inject] IJSRuntime JS { get; set; }
        [Inject] ILocalStorageService LocalStorage { get; set; }

        protected static Func<float, float, Task> getWeatherFunc;
        protected string errorMessage = null;
        protected bool unauthorized = false;
        protected bool useCity = false;
        protected string city = null;
        protected bool IsVisibleWeatherComponent;
        protected DateTimeFormatType DateTimeFormat { get; set; }
        protected CurrentWeather CurrentWeather { get; set; }
        protected UnitsType Units;


        private void Timer_Elapsed(object _, ElapsedEventArgs e)
        {
            RefreshHandler().Wait();
        }

        public async Task RefreshHandler()
        {
            await GetCurrentWeather(CurrentWeather.coord.lat, CurrentWeather.coord.lon);
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            getWeatherFunc = GetWeather;
        }

        protected override async Task OnInitializedAsync()
        {
            useCity = await LocalStorage.GetItemAsync<bool>(Constants.KeyName_UseCity);
            city = await LocalStorage.GetItemAsync<string>(Constants.KeyName_City);

            var storedRefreshMinutes = await LocalStorage.GetItemAsync<int?>(Constants.KeyName_RefreshTime);
            int refreshMinutes = storedRefreshMinutes is null
                                    ? Constants.DefaultValue_Refresh
                                    : Convert.ToInt32(storedRefreshMinutes);

            bool? storedShowWeather = await LocalStorage.GetItemAsync<bool?>(Constants.KeyName_ShowWeather);
            IsVisibleWeatherComponent = storedShowWeather ?? true;

            DateTimeFormat = await LocalStorage.GetItemAsync<DateTimeFormatType?>(Constants.KeyName_TimeFormat) ?? DateTimeFormatType.T24hs;
           
            var unitsConfig = await LocalStorage.GetItemAsync<UnitsType?>(Constants.KeyName_Units);
            Units = unitsConfig ?? UnitsType.Metric;

            if (refreshMinutes > 0)
            {
                Timer timer = new();
                timer.Interval = 1000 * 60 * refreshMinutes;
                timer.Elapsed += Timer_Elapsed;
                timer.Enabled = true;
            }

            await GetGeoLocation();
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
                    CurrentWeather = await OpenWeather.GetCurrentWeatherByCity(city, Units.ToString().ToLower());
                }
                else
                {
                    CurrentWeather = await OpenWeather.GetCurrentWeatherByCoords(lat, lon, Units.ToString().ToLower());
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
