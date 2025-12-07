namespace AdventOfCode2025.Helpers
{
    public static class MatrixUtils
    {
        public static T[,] ToMatrix<T>(this T[][] array)
        {
            var matrix = new T[array.Length, array[0].Length];

            for (var y = 0; y < array.Length; y++)
            for (var x = 0; x < array[0].Length; x++)
                matrix[y, x] = array[y][x];

            return matrix;
        }

        public static T[,] Transpose<T>(this T[,] matrix)
        {
            var m = matrix.GetLength(0);
            var n = matrix.GetLength(1);

            var newMatrix = new T[n, m];
            for (var y = 0; y < n; y++)
            for (var x = 0; x < m; x++)
                newMatrix[y, x] = matrix[x, y];

            return newMatrix;
        }

        public static (int, int) First<T>(this T[,] matrix, Func<T, bool> pred)
        {
            for (var y = 0; y < matrix.GetLength(0); y++)
            for (var x = 0; x < matrix.GetLength(1); x++)
                if (pred(matrix[y, x]))
                    return (y, x);
            return (-1, -1);
        }

        public static int Count<T>(this T[,] matrix, Func<T, bool> pred)
        {
            var counter = 0;

            for (var y = 0; y < matrix.GetLength(0); y++)
            for (var x = 0; x < matrix.GetLength(1); x++)
                if (pred(matrix[y, x]))
                    counter++;

            return counter;
        }

        public static void OnEachInMatrix<T>(this T[,] matrix, Func<T, T> operation)
        {
            for(var y = 0; y < matrix.GetLength(0); y++)
            for (var x = 0; x < matrix.GetLength(1); x++)
                matrix[y, x] = operation(matrix[y, x]);
        }

        public static T[,] CopyMatrix<T>(this T[,] matrix) => matrix.Clone() as T[,];

        public static T[] GetRow<T>(this T[,] matrix, int rowY)
        {
            var array = new T[matrix.GetLength(1)];

            for (var x = 0; x < matrix.GetLength(1); x++)
                array[x] = matrix[rowY, x];

            return array;
        }

        public static T[] GetColumn<T>(this T[,] matrix, int columnX)
        {
            var array = new T[matrix.GetLength(0)];

            for (var y = 0; y < matrix.GetLength(0); y++)
                array[y] = matrix[y, columnX];

            return array;
        }

        public static int[,] ToMatrix(this string[] array)
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
    }
}
