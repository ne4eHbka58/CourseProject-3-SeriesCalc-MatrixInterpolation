using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Logic
{
    public static class Arrays
    {

        /// <summary>
        /// Создание массива A
        /// </summary>
        public static double[] MakeArrayA(double iStart, double iEnd, double hstep, double epsilon, int dimension, int precision)
        {
            double[] A = new double[dimension];

            int arrayIndex = 0;

            if (hstep > 0) // Проверка на ввод отрицательного начального значения шага
            {
                for (double i = iStart; i <= iEnd; i += hstep)
                {
                    A[arrayIndex] = Math.Round(SeriesCalc.SeriesSum(i, epsilon), precision); // Заполнение массива A с округлением до точности epsilon
                    arrayIndex++;
                }
            }
            else
            {
                for (double i = iStart; i >= iEnd; i += hstep)
                {
                    A[arrayIndex] = Math.Round(SeriesCalc.SeriesSum(i, epsilon), precision); // Заполнение массива A с округлением до точности epsilon
                    arrayIndex++;
                }
            }
            return A;
        }

        /// <summary>
        /// Создание массива контрольных сумм
        /// </summary>
        public static double[] MakeArrayControlFormula(double iStart, double iEnd, double hstep, double epsilon, int dimension, int precision)
        {
            double[] checkSums = new double[dimension];

            int arrayIndex = 0;

            if (hstep > 0) // Проверка на ввод отрицательного начального значения шага
            {
                for (double i = iStart; i <= iEnd; i += hstep)
                {
                    checkSums[arrayIndex] = Math.Round(SeriesCalc.ControlFormula(i), precision); // Заполнение массива контрольных сумм
                    arrayIndex++;
                }
            }
            else
            {
                for (double i = iStart; i >= iEnd; i += hstep)
                {
                    checkSums[arrayIndex] = Math.Round(SeriesCalc.ControlFormula(i), precision); // Заполнение массива контрольных сумм
                    arrayIndex++;
                }
            }
            return checkSums;
        }

        /// <summary>
        /// Создание массива B
        /// </summary>
        public static double[,] MakeArrayB(int dimension)
        {
            double[,] B = new double[dimension, dimension];
            int value = 1;
            for (int i = dimension - 1; i >= 0; i--)
            {
                for (int j = 0; j < dimension; j++)
                {
                    if (i % 2 == (dimension - 1) % 2)
                    {
                        B[i, j] = value;
                    }
                    else
                    {
                        B[i, dimension - 1 - j] = value;
                    }
                    value++;
                }
            }
            return B;
        }
        /// <summary>
        /// Создание массива C путём перемножения одномерного и двумерного массива и домножения на 2
        /// </summary>
        public static double[] MakeArrayC(double[,] B, double[] A)
        {
            int dimension = A.Length;

            double[] C = new double[dimension];

            for (int i = 0; i < dimension; i++)
            {
                double result = 0;
                for (int j = 0; j < dimension; j++)
                {
                    result += A[j] * B[j, i];
                }
                C[i] = result * 2;
            }
            return C;
        }

        /// <summary>
        /// Создание массива Y
        /// </summary>
        public static double[,] MakeArrayY(double gstep, int precision, double[] C)
        {
            double max = C.Max();
            double min = C.Min();
            int Yprecision = (int)Math.Ceiling(-Math.Log10(gstep));
            int count = (int)(C.Length*1/gstep) + 1; // Высчитывает количество возможных шагов по массиву C
            double[,] Y = new double[count, 2];

            double iLag = 0;
            for (int arrayIndex = 0; arrayIndex < count; arrayIndex++)
            {
                Y[arrayIndex, 0] = Math.Round(iLag, Yprecision);
                Y[arrayIndex, 1] = Lagranj(C, iLag);
                iLag += gstep;
            }
            return Y;
        }

        public static double Lagranj(double[] x, double t)
        {
            double sum = 0;
            int n = x.Length;
            double p;

            for (int i = 0; i < n; i++)
            {
                p = 1;
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        p = p * (t - j) / (i - j);
                    }
                }
                sum += x[i] * p;
            }
            return sum;
        }
        public static double[,] Sort(double[,] matrix)
        {
            if (matrix == null || matrix.GetLength(1) != 2)
            {
                // Обработка null или неверный размерности матрицы
                return matrix;
            }

            int rows = matrix.GetLength(0);
            double[,] sortedMatrix = new double[rows, 2];
            Array.Copy(matrix, sortedMatrix, matrix.Length);  // Копия для сортировки

            MergeSortRecursive(sortedMatrix, 0, rows - 1);

            return sortedMatrix;
        }


        private static void MergeSortRecursive(double[,] arr, int left, int right)
        {
            if (left < right)
            {
                int middle = left + (right - left) / 2;

                MergeSortRecursive(arr, left, middle);
                MergeSortRecursive(arr, middle + 1, right);

                Merge(arr, left, middle, right);
            }
        }

        private static void Merge(double[,] arr, int left, int middle, int right)
        {
            int leftSize = middle - left + 1;
            int rightSize = right - middle;

            // Временные массивы
            double[,] leftArray = new double[leftSize, 2];
            double[,] rightArray = new double[rightSize, 2];

            // Копирование данных в временные массивы
            for (int i = 0; i < leftSize; i++)
            {
                leftArray[i, 0] = arr[left + i, 0];
                leftArray[i, 1] = arr[left + i, 1];
            }
            for (int j = 0; j < rightSize; j++)
            {
                rightArray[j, 0] = arr[middle + 1 + j, 0];
                rightArray[j, 1] = arr[middle + 1 + j, 1];
            }


            int ii = 0, jj = 0, k = left;
            while (ii < leftSize && jj < rightSize)
            {
                if (leftArray[ii, 1] <= rightArray[jj, 1])  // Сортировка по второму столбцу
                {
                    arr[k, 0] = leftArray[ii, 0];
                    arr[k, 1] = leftArray[ii, 1];
                    ii++;
                }
                else
                {
                    arr[k, 0] = rightArray[jj, 0];
                    arr[k, 1] = rightArray[jj, 1];
                    jj++;
                }
                k++;
            }

            // Копирование оставшихся элементов
            while (ii < leftSize)
            {
                arr[k, 0] = leftArray[ii, 0];
                arr[k, 1] = leftArray[ii, 1];
                ii++;
                k++;
            }

            while (jj < rightSize)
            {
                arr[k, 0] = rightArray[jj, 0];
                arr[k, 1] = rightArray[jj, 1];
                jj++;
                k++;
            }
        }
    }
}
