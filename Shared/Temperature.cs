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
}
