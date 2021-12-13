using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public static class StringExtensions
{
    public static string ToTitleCase(this string source)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);
    }

    public static string ToProperCase(this string source)
    {
        return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source);
    }
    public static string FixQuotes(this string s)
    {
        return s.Replace("'", "''");
    }

    public static string FormatWith(this string format, params object[] args)
    {
        return string.Format(format, args);
    }

}
