using BiDegree.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BiDegree.Features.PhotoFrame
{
    public partial class PhotoFramePage : IDisposable
    {
        [Inject] ILocalStorageService LocalStorage { get; set; }

        private Clock4 clock;
        private Timer timer;
        private readonly Counters counters = new();
        private Slideshow slideshow = new();
        //private DateTime actionTime;
        private const int DelayToLoadNextInBackground = 2;
        private const int clockTick = 2;
        private TimeFormatType TimeFormat;
        private DateFormatType DateFormat;
        private TemperatureFormatType TemperatureFormat;
        protected override async Task OnInitializedAsync()
        {
            await InitializeCounters();

            actionTime = DateTime.Now;

            timer = new Timer(async (e) =>
            {
                await Task.Delay(500);
                await TimerElapsed();
            }, null, 0, 1000);

        }

        private async Task InitializeCounters()
        {
            var storedDuration = await LocalStorage.GetItemAsync<int?>(Constants.KeyName_ShowTime);

            var setShowTimeTask = counters.NextItem.Initialize(
                storedDuration is null
                            ? Constants.DefaultValue_ShowTime
                            : (int)storedDuration
                            );

            storedDuration = await LocalStorage.GetItemAsync<int?>(Constants.KeyName_RefreshTime);

            var setCheckWeatherTask = counters.CheckWeather.Initialize(
                storedDuration is null
                            ? Constants.DefaultValue_Refresh
                            : (int)storedDuration
                            );

            TimeFormat = await LocalStorage.GetItemAsync<TimeFormatType?>(Constants.KeyName_TimeFormat) ?? TimeFormatType.T24hs;
            DateFormat = await LocalStorage.GetItemAsync<DateFormatType?>(Constants.KeyName_DateFormat) ?? DateFormatType.Date1_xWD_M_D;
            TemperatureFormat = await LocalStorage.GetItemAsync<TemperatureFormatType?>(Constants.KeyName_TempFormat) ?? TemperatureFormatType.CF;

            var setLoadBackgroundDelayTask = counters.LoadBackgroundItem.Initialize(DelayToLoadNextInBackground);
            var setClockTickTask = counters.Clock.Initialize(clockTick);

            await Task.WhenAll(setShowTimeTask, setCheckWeatherTask, setLoadBackgroundDelayTask, setClockTickTask);

        }

        private async Task TimerElapsed()
        {

            //if ((DateTime.Now - actionTime).TotalSeconds < 1) { return; }

            //actionTime = DateTime.Now;

            await counters.Update();

            if (counters.LoadBackgroundItem.IsExpired)
            {
                await slideshow.LoadNextInBackground();
                counters.LoadBackgroundItem.Duration = int.MaxValue;
            }

            if (counters.NextItem.IsExpired)
            {
                await slideshow.ShowNext();

                counters.NextItem.Reset();
                counters.LoadBackgroundItem.Reset();
            }

            if (counters.Clock.IsExpired)
            {
                counters.Clock.Reset();
             
                if (TimeFormat != TimeFormatType.None)
                {
                    clock.Update();
                }
            }

            if (counters.CheckWeather.IsExpired)
            {
                counters.CheckWeather.Reset();

                if(TemperatureFormat != TemperatureFormatType.None)
                {
                    // TODO: Update weather
                }
            }

        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }

    class Counters
    {

        public SecondsCounter Clock { get; set; } = new();
        public SecondsCounter CheckWeather { get; set; } = new();
        public SecondsCounter NextItem { get; set; } = new();
        public SecondsCounter LoadBackgroundItem { get; set; } = new();

        public async Task Update()
        {
            await Task.Factory.StartNew(() =>
            {
                Clock.Duration--;
                CheckWeather.Duration--;
                NextItem.Duration--;
                LoadBackgroundItem.Duration--;

            });
        }
    }

    class SecondsCounter
    {
        public int InitialValue { get; set; }
        public int Duration { get; set; }
        public bool IsExpired { get => Duration == 0; }

        public async Task Initialize(int initialValue)
        {
            await Task.Factory.StartNew(() =>
            {
                InitialValue = initialValue;
                Duration = initialValue;
            });
        }

        public void Reset()
        {
            Duration = InitialValue;
        }
    }
}
