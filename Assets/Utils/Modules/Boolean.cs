using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static partial class Utils
{
    public static int TrueCount(this bool[] bArray)
    {
        return bArray.CountOf(true);
    }
    public static int FalseCount(this bool[] bArray)
    {
        return bArray.CountOf(false);
    }
    public static int CountOf(this bool[] bArray, bool identifier)
    {
        return bArray.Where(t => t == identifier).Count();
    }
}