using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace BiDegree.Shared
{
    public partial class Clock : IDisposable
    {
        [Inject] ILocalStorageService localStorage { get; set; }

        private string DisplayDate;
        private string DisplayTime;
        private string AmPm;
        private TimeFormatType timeFormat = TimeFormatType.T12hs;
        private Timer Timer { get; set; }
        public Clock()
        {
   
        }

        protected override async Task OnInitializedAsync()
        {
            Timer = new();
            Timer.Interval = 5000;
            Timer.Elapsed += Timer_Elapsed;
            Timer.Enabled = true;

            SetDateTime();

            timeFormat = await localStorage.GetItemAsync<TimeFormatType?>(Constants.KeyName_TimeFormat) ?? TimeFormatType.T12hs;
        }

        private void Timer_Elapsed(object _, ElapsedEventArgs e)
        {
            SetDateTime();
            StateHasChanged();
        }

        private void SetDateTime()
        {
            DateTime now = DateTime.Now;
            DisplayDate = $"{now.DayOfWeek.ToString()[..3].ToUpper()}, {now:MMM} {now:dd}";

            if (timeFormat == TimeFormatType.T12hs)
            {
                DisplayTime = now.ToString("h:mm");
                AmPm = now.ToString("tt");
                return;
            }
            
            DisplayTime = now.ToString("HH:mm");
            AmPm = "";
        }

        public void Dispose()
        {
            Timer?.Dispose();
        }
    }
}

