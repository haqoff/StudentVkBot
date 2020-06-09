using System;
using System.Collections.Generic;
using System.Diagnostics;
using VkBotHelper.Command;

namespace StudentBotCore.Helpers
{
    public static class Helper
    {
        public static IList<T> InsertionSortInPlace<T>(this IList<T> list, Func<T, T, int> compareFunc)
        {
            for (var i = 0; i < list.Count - 1; i++)
            {
                for (var j = i + 1; j > 0; j--)
                {
                    if (compareFunc(list[j - 1], list[j]) > 0)
                    {
                        var temp = list[j - 1];
                        list[j - 1] = list[j];
                        list[j] = temp;
                    }
                }
            }

            return list;
        }

        public static bool Find<T>(this IList<T> items, Func<T, bool> func, out T item)
        {
            for (var i = 0; i < items.Count; i++)
            {
                var cur = items[i];
                if (!func(cur)) continue;

                item = cur;
                return true;
            }

            item = default;
            return false;
        }

        public static void InsertToEnd<T>(ref T[] array, T item)
        {
            Array.Resize(ref array, array.Length + 1);
            array[^1] = item;
        }

        public static int BinarySearch<T>(IList<T> list, Func<T, int> compareFunc)
        {
            var left = 0;
            var right = list.Count - 1;

            while (left <= right)
            {
                var mid = left + (right - left) / 2;
                var el = list[mid];

                var compare = compareFunc(el);
                if (compare == 0) return mid;

                if (compare < 0) right = mid - 1;
                else left = mid + 1;
            }

            return -1;
        }
    }
}