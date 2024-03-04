using System;
using System.Collections.Generic;

public static class Search
{
    public static int Binary<T>(IList<T> list, T value)
    {
        if (list == null)throw new ArgumentNullException("list");

        var comp = Comparer<T>.Default;
        int lo = 0, hi = list.Count - 1;
        while (lo < hi)
        {
            int m = (hi + lo) / 2;  // this might overflow; be careful.
            if (comp.Compare(list[m], value) < 0) lo = m + 1;
            else hi = m - 1;
        }
        
        if (comp.Compare(list[lo], value) < 0) lo++;
        return lo;
    }
}