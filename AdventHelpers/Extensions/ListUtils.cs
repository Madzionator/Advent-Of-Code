using System.Collections.Generic;

namespace Advent.Helpers.Extensions
{
    public static class ListUtils
    {
        public static void Move<T>(this List<T> list, int oldIdx, int newIdx)
        {
            if (oldIdx == newIdx)
                return;

            T item = list[oldIdx];
            list.RemoveAt(oldIdx);
            list.Insert(newIdx, item);
        }
    }
}