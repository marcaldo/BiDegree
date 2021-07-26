using System;
using System.Globalization;

namespace BiDegree.Shared
{
    public static class Temperature
    {
        public static float ToFahrenheit(this float celsius) => ((celsius * 9 / 5) + 32);
        public static float ToCelsius(this float fahrenheit) => ((fahrenheit - 32) * 5 / 9);
        public static string ToFahrenheitString(this float celsius) => ToFahrenheit(celsius).ToDecimalString();
        public static string ToCelsiusString(this float fahrenheit) => ToCelsius(fahrenheit).ToDecimalString();
        public static string ToDecimalString(this float value) => value.ToString("0", CultureInfo.InvariantCulture);
    }

    public enum UnitsType
    {
        Metric,
        Imperial
    }

    [Flags]
    public enum TimeFormatType
    {
        None,
        T24hs = 1 << 0,     // Of course we should never have 
        T12hs = 1 << 1,     // this both flags at the same time.
        ShowDate = 1 << 2
    }
}
