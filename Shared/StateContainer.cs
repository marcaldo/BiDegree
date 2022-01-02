using BiDegree.Models;
using System;

namespace BiDegree.Shared
{
    public class StateContainer
    {
        private float currentTemp;
        private DisplayWeatherWidgetType displayWeatherWidgetType;

        public float CurrentTemp
        {
            get => currentTemp;
            set
            {
                currentTemp = value;
                NotifyStateChanged();
            }
        }
        public DisplayWeatherWidgetType DisplayWeatherWidgetType
        {
            get => DisplayWeatherWidgetType;
            set
            {
                displayWeatherWidgetType = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}