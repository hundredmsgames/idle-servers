using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static string Format(int num)
    {
        if (num < 1000)
            return num.ToString();
        else
            return RemoveTrailingZero((num / 1000f).ToString("n1")) + "K";


        //if (num >= 100000000)
        //    return (num / 1000000).ToString("#,0M");
        //if (num >= 10000000)
        //    return (num / 1000000).ToString("0.#") + "M";
        //if (num >= 100000)
        //    return (num / 1000).ToString("#,0K");
        //if (num >= 10000)
        //    return (num / 1000).ToString("0.#") + "K";
        //if( num >= 1000)
        //    return (num / 1000).ToString("0.#K");

        //return (num).ToString("0,#");
    }
    /// <summary>
    /// Gets an integer and calculates the hours and minutes based on seconds. Returns formatted string like 00:00:00
    /// </summary>
    /// <param name="seconds"></param>
    /// <returns></returns>
    public static string TimerFormat(this int seconds)
    {
        int hours, mins;
        hours = seconds / 360;
        seconds -= hours * 360;
        mins = seconds / 60;
        seconds -= mins * 60;

        return string.Format("{0:00}:{1:00}:{2:00}", hours, mins, seconds);
    }

    public static string RemoveTrailingZero(string num)
    {
        if (num[num.Length - 1] == '0')
            return num.Substring(0, num.Length - 2);

        return num;
    }
}
