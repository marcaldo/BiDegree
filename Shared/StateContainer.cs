using BiDegree.Models;
using System;
using System.Collections.Generic;

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
        public float CurrentTemp
        {
            get => currentTemp;
            set
            {
                currentTemp = value;
                NotifyStateChanged();
            }
        }

        private DisplayWeatherWidgetType displayWeatherWidgetType;
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

        private DeviceMeasurement deviceMeasurement;
        public DeviceMeasurement DeviceMeasurement
        {
            get => deviceMeasurement;
            set
            {
                deviceMeasurement = value;
                NotifyStateChanged();
            }
        }


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