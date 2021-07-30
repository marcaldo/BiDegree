﻿using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BiDegree.Shared
{
    public partial class Clock : IDisposable
    {
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        private int _count = 0;

        private Timer _timer;
        private DateTimeDisplay _dateTimeDisplay = new();

        protected override async Task OnInitializedAsync()
        {
            _dateTimeDisplay.timeFormat = await LocalStorage.GetItemAsync<DateTimeFormatType?>(Constants.KeyName_TimeFormat) ?? DateTimeFormatType.T12hs;
            SetTimer();
        }

        private void SetTimer()
        {
            _timer = new Timer(
                        callback: new TimerCallback(TimerElapsed),
                        state: _dateTimeDisplay,
                        dueTime: 0,
                        period: 5000);
        }

        private void TimerElapsed(object timerState)
        {
            _dateTimeDisplay = timerState as DateTimeDisplay;

            DateTime now = DateTime.Now;
            _dateTimeDisplay.Date = $"{now.DayOfWeek.ToString()[..3].ToUpper()}, {now:MMM} {now:dd}";

            if (_dateTimeDisplay.timeFormat == DateTimeFormatType.T12hs)
            {
                _dateTimeDisplay.Time = now.ToString("h:mm:ss");
                _dateTimeDisplay.AmPm = now.ToString("tt");
            }
            else
            {
                _dateTimeDisplay.Time = now.ToString("HH:mm:ss");
                _dateTimeDisplay.AmPm = "";
            }

            //++_count;
            //if (_count >= 3)
            //{
            //    _count = 0;
            _timer.Dispose();
            SetTimer();
            //}

            StateHasChanged();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    class DateTimeDisplay
    {
        public string Time { get; set; }
        public string AmPm { get; set; }
        public string Date { get; set; }
        public DateTimeFormatType timeFormat { get; set; } = DateTimeFormatType.T24hs;
    }
}

