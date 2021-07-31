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



    public enum TimeFormatType
    {
        None = 0,
        T24hs = 1,
        T12hs = 2
    }

    public enum DateFormatType
    {
        None = 0,
        Date1_xWD_M_D = 1,    // Date1: TUE, Set 23
        Date2_WD_D = 2,       // Date2: Tuesday 23
        Date3_WD = 3,         // Date3: Tuesday
        Date4_DD_MMM_YY = 4,  // Date4: 23 SEP 2021
        Date5_MMM_DD_YY = 5,  // Date5: SEP 23 2021
        Date6_DD_MM_YY = 6,   // Date6: 23/09/21
        Date7_MM_DD_YY = 7,   // Date7: 09/23/21
    }

    public enum TempFormatType
    {
        None = 0,
        C = 1,
        CF = 2,
        F = 3,
        FC = 4
    }
}
