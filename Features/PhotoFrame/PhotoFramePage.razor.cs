using BiDegree.Features.Settings;
using BiDegree.Models;
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

        private Timer timer;
        private Counters counters;
        private Slideshow slideshow;
        private const int DelayToLoadNextInBackground = 2;

        protected override void OnInitialized()
        {
            //timer = new Timer(
            //    callback: new TimerCallback(TimerElapsed),
            //    state: null,
            //    dueTime: 0,
            //    period: 1000);
        }

        protected override async Task OnInitializedAsync()
        {
            counters = new Counters();
            await InitializeCounters();

            timer = new Timer(async (e) =>
            {
                await Task.Delay(500);
                await TimerElapsed();
            }, null, 0, 1000);

        }

        private async Task InitializeCounters()
        {
            var storedDuration = await LocalStorage.GetItemAsync<int?>(Constants.KeyName_ShowTime);
            await counters.NextItem.Initialize(
                storedDuration is null
                            ? Constants.DefaultValue_ShowTime
                            : (int)storedDuration
                            );

            await counters.LoadBackgroundItem.Initialize(
                counters.NextItem.Duration + DelayToLoadNextInBackground
                );

            storedDuration = await LocalStorage.GetItemAsync<int?>(Constants.KeyName_RefreshTime);
            await counters.CheckWeather.Initialize(
                storedDuration is null
                            ? Constants.DefaultValue_Refresh
                            : (int)storedDuration
                            );


        }

        private async Task TimerElapsed()
        {
            await counters.Update();

            if (counters.LoadBackgroundItem.IsExpired)
            {
                await slideshow.LoadNextInBackground();
                counters.LoadBackgroundItem.Reset();
            }

            if (counters.NextItem.IsExpired)
            {
                slideshow.ShowNext();
                counters.NextItem.Reset();

                Console.WriteLine("NextItem " + counters.NextItem.Duration);
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

                Console.WriteLine("LoadBackgroundItem " + LoadBackgroundItem.Duration);
                Console.WriteLine("NextItem " + NextItem.Duration);


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
