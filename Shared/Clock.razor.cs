using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace BiDegree.Shared
{
    public partial class Clock
    {
        [Inject] ILocalStorageService localStorage { get; set; }

        private string DisplayDate;
        private string DisplayTime;
        private string AmPm;
        private TimeFormatType timeFormat = TimeFormatType.T12hs;
        public Clock()
        {
            Timer timer = new();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            SetDateTime();
        }

        protected override async Task OnInitializedAsync()
        {
            timeFormat = await localStorage.GetItemAsync<TimeFormatType?>(Constants.TimeFormat) ?? TimeFormatType.T12hs;
        }

        private void Timer_Elapsed(object _, ElapsedEventArgs e)
        {
            SetDateTime();
            StateHasChanged();
        }

        private void SetDateTime()
        {
            DateTime now = DateTime.Now;
            DisplayDate = $"{now.DayOfWeek.ToString()[..3].ToUpper()}, {now:MMM} {now:dd}, '{now:yy}";

            if (timeFormat == TimeFormatType.T12hs)
            {
                DisplayTime = now.ToString("h:mm");
                AmPm = now.ToString("tt");
                return;
            }
            
            DisplayTime = now.ToString("HH:mm");
            AmPm = "";
        }
    }
}

