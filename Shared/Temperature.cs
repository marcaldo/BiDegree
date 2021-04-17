using System.Globalization;

namespace BiDegree.Shared
{
    public static class Temperature
    {

        public static string ToFahrenheit(this float celsius) => ((celsius * 9 / 5) + 32).ToDecimal();
        public static string ToCelsius(this float fahrenheit) => ((fahrenheit - 32) * 5 / 9).ToDecimal();
        public static string ToDecimal(this float value) => value.ToString("0.0", CultureInfo.InvariantCulture);
    }

    public enum UnitsType
    {
        Metric,
        Imperial
    }
}
