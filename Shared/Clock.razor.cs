using System;
using System.Timers;

namespace BiDegree.Shared
{
    public partial class Clock
    {
        private string DisplayDate { get; set; }
        private string DisplayTime { get; set; }
        public Clock()
        {
            Timer timer = new();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;

            SetDateTime();
        }

        private void Timer_Elapsed(object _, ElapsedEventArgs e)
        {
            SetDateTime();
            StateHasChanged();
        }

        private void SetDateTime()
        {
            DateTime now = DateTime.Now;
            DisplayDate = $"{now.DayOfWeek.ToString()[..3].ToUpper()}, {now.ToString("MMM")} {now:dd}, '{now:yy}";            
            DisplayTime = now.ToString("t");
        }
    }
}
