using BiDegree.Models;
using System;

namespace BiDegree.Shared
{
    public sealed class StateContainer
    {
        private static StateContainer instance;
        public static StateContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StateContainer();
                }

                return instance;
            }
        }

        private float currentTemp;
        private float measuredTemp;
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

        public float MeasuredTemp
        {
            get => measuredTemp;
            set
            {
                measuredTemp = value;
                NotifyStateChanged();
            }
        }
        public DisplayWeatherWidgetType DisplayWeatherWidgetType
        {
            get => displayWeatherWidgetType;
            set
            {
                displayWeatherWidgetType = value;
                NotifyStateChanged();
            }
        }

        public WeatherStatus WeatherStatus { get; set; } = new WeatherStatus();


        public event Action OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

    public class WeatherStatus
    {
        public DateTime LastUpdated { get; set; }
        public DateTime NextWeatherApiCall { get; set; } = DateTime.Now;
        public CurrentWeather CurrentWeather { get; set; }
    }
}