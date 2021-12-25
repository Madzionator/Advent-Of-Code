using System;

namespace Advent.Helpers.Extensions
{
    public static class MatrixUtils
    {
        public static int[,] ToMatrix (this string[] array)
        {
            var matrix = new int[array.Length, array[0].Length];

            for (var y = 0; y < array.Length; y++)
            for (var x = 0; x < array[0].Length; x++)
                matrix[y, x] = array[y][x] - '0';

            return matrix;
        }

        public static char[,] ToCharMatrix(this string[] array)
        {
            var matrix = new char[array.Length, array[0].Length];
            if (array.Length == 0)
                return matrix;

            for (var y = 0; y < array.Length; y++)
            for (var x = 0; x < array[0].Length; x++)
                matrix[y, x] = array[y][x];

            return matrix;
        }

        public static void OnEachInMatrix<T>(this T[,] matrix, Func<T, T> operation)
        {
            for(var y = 0; y < matrix.GetLength(0); y++)
            for (var x = 0; x < matrix.GetLength(1); x++)
                matrix[y, x] = operation(matrix[y, x]);
        }

        public static void DrawMatrix<T>(this T[,] matrix, string separator = "")
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
            {
                for (var x = 0; x < matrix.GetLength(1); x++)
                    Console.Write(matrix[y, x].ToString() + separator);
                Console.WriteLine();
            }
        }

        public static T[,] CopyMatrix<T>(this T[,] matrix) => matrix.Clone() as T[,];
    }
}
