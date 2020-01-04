using System;
using System.Collections.Generic;
using System.Linq;

namespace PathfindingVisualisation
{
    public static class ArrayExtensions
    {
        private const int seedPrimeNumber = 17;

        private const int fieldPrimeNumber = 23;


        public static int GetHeight<T>(this T[,] array) => array.GetLength(0);

        public static int GetWidth<T>(this T[,] array) => array.GetLength(1);

        public static int GetHashCodeFromArray2D<T>(this T[,] array)
        {
            unchecked
            {
                var hash = seedPrimeNumber;
                var height = array.GetHeight();
                var width = array.GetWidth();
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        hash = (hash * fieldPrimeNumber) + (array[y, x]?.GetHashCode() ?? 0);
                    }
                }
                return hash;
            }
        }

        public static bool Equals2D<T>(this T[,] array, T[,] other)
        {
            return array.Cast<T>().SequenceEqual(other.Cast<T>());
        }

        public static T[] ToArray<T>(this T[,] array)
        {
            return array.Cast<T>().ToArray();
        }

        public static T[,] ToArray2D<T>(this IEnumerable<T> data, int height, int width)
        {
            var i = 0;
            var newArray = new T[height, width];
            foreach (var item in data)
            {
                var y = i / width;
                var x = i % width;
                newArray[y, x] = item;
                i++;
            }
            return newArray;
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this TSource[,] array, Func<TSource, TResult> selector)
        {
            var height = array.GetHeight();
            var width = array.GetWidth();
            var newArray = new TResult[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    yield return selector(array[y, x]);
                }
            }
        }

        public static TResult[,] Select2D<TSource, TResult>(this TSource[,] array, Func<TSource, TResult> selector)
        {
            var height = array.GetHeight();
            var width = array.GetWidth();
            var newArray = new TResult[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    newArray[y, x] = selector(array[y, x]);
                }
            }
            return newArray;
        }

        public static T[,] GetSubArray2D<T>(this T[,] array, int offsetY, int offsetX, int height, int width)
        {
            var newArray = new T[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    newArray[y, x] = array[offsetY + y, offsetX + x];
                }
            }
            return newArray;
        }

        public static T[,] GetSubArraySafe2D<T>(this T[,] array, int offsetY, int offsetX, int height, int width, T defaultValue)
        {
            var arrayHeight = array.GetHeight();
            var arrayWidth = array.GetWidth();
            var newArray = new T[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var arrayY = offsetY + y;
                    var arrayX = offsetX + x;
                    if (arrayY < 0 || arrayY >= arrayHeight || arrayX < 0 || arrayX >= arrayWidth)
                    {
                        newArray[y, x] = defaultValue;
                    }
                    else
                    {
                        newArray[y, x] = array[arrayY, arrayX];
                    }
                }
            }
            return newArray;
        }

        public static void SetSubArray2D<T>(this T[,] array, int offsetY, int offsetX, T[,] subArray)
        {
            var subArrayHeight = subArray.GetHeight();
            var subArrayWidth = subArray.GetWidth();
            for (var y = 0; y < subArrayHeight; y++)
            {
                for (var x = 0; x < subArrayWidth; x++)
                {
                    array[offsetY + y, offsetX + x] = subArray[y, x];
                }
            }
        }

        public static void SetSubArraySafe2D<T>(this T[,] array, int offsetY, int offsetX, T[,] subArray)
        {
            var arrayHeight = array.GetHeight();
            var arrayWidth = array.GetWidth();
            var subArrayHeight = subArray.GetHeight();
            var subArrayWidth = subArray.GetWidth();
            for (var y = 0; y < subArrayHeight; y++)
            {
                for (var x = 0; x < subArrayWidth; x++)
                {
                    var arrayY = offsetY + y;
                    var arrayX = offsetX + x;
                    if (arrayY < 0 || arrayY >= arrayHeight || arrayX < 0 || arrayX >= arrayWidth)
                    {
                        continue;
                    }
                    array[arrayY, arrayX] = subArray[y, x];
                }
            }
        }
    }
}
