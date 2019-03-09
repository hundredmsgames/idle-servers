﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static string Format(this int num)
    {
        if (num >= 100000000)
            return (num / 1000000).ToString("#,0M");
        if (num >= 10000000)
            return (num / 1000000).ToString("0.#") + "M";
        if (num >= 100000)
            return (num / 1000).ToString("#,0K");
        if (num >= 10000)
            return (num / 1000).ToString("0.#") + "K";
        if( num >=1000)
            return (num / 1000).ToString("0.#K");
        return (num).ToString("0,#");
    }
}
