using LiveCharts.Wpf;
using LiveCharts;
using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Model;
using WpfApp.ViewModel;
using System.Data;
using System.Security.Policy;
using System.Runtime.Serialization;
using LiveCharts.Helpers;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp.FileHelper
{
    public class FileFiller
    {
        string filePath;
        Dan initialData;
        VmMainWindow VmMainW;
        
        public FileFiller(string filePath, Dan initialData, VmMainWindow DataContext)
        {
            this.filePath = filePath;
            this.initialData = initialData;
            VmMainW = DataContext;
        }

        public void FillFile()
        {
            using (StreamWriter sw = new StreamWriter(filePath, append: false))
            {
                sw.WriteLine($"Х начальное: {initialData.iStart}");
                sw.WriteLine($"Х конечное: {initialData.iEnd}");
                sw.WriteLine($"Шаг x: {initialData.hstep}");
                sw.WriteLine($"Точность: {initialData.epsilon}");
                sw.WriteLine($"Шаг интерполяции (g): {initialData.gstep}");

                sw.WriteLine("\nМассив A\n");
                initialData.A = Arrays.MakeArrayA(initialData.iStart, initialData.iEnd, initialData.hstep, initialData.epsilon, initialData.dimension, initialData.precision);
                initialData.checkSums = Arrays.MakeArrayControlFormula(initialData.iStart, initialData.iEnd, initialData.hstep, initialData.epsilon, initialData.dimension, initialData.precision);

                sw.WriteLine($"x\ty\tКонтрольная формула");

                int arrayIndex = 0;
                //Запись массива A и массива контрольных формул в файл
                if (initialData.hstep > 0) // Проверка на ввод отрицательного начального значения
                {
                    for (double i = initialData.iStart; i <= initialData.iEnd; i += initialData.hstep)
                    {
                        sw.WriteLine($"{Math.Round(i, initialData.precision)}\t{initialData.A[arrayIndex]}\t{initialData.checkSums[arrayIndex]}");
                        arrayIndex++;
                    }
                }
                else
                {
                    for (double i = initialData.iStart; i >= initialData.iEnd; i += initialData.hstep)
                    {
                        sw.WriteLine($"{Math.Round(i, 1)}\t{initialData.A[arrayIndex]}\t{initialData.checkSums[arrayIndex]}");
                        arrayIndex++;
                    }
                }

                VmMainW.ListA = VmMainW.GetListFromArray(initialData.A, initialData.checkSums); // Добавление массива в коллекцию для вывода в DataGrid


                sw.WriteLine("\nМассив B\n");

                initialData.B = Arrays.MakeArrayB(initialData.dimension);

                for (int i = 0; i < initialData.dimension; i++)
                {
                    for (int j = 0; j < initialData.dimension; j++)
                    {
                        sw.Write($"{initialData.B[i, j]}\t");
                    }
                    sw.WriteLine();
                }

                VmMainW.ListB = VmMainW.GetListFromArrayB(initialData.B);


                sw.WriteLine("\nМассив C\n");

                initialData.C = Arrays.MakeArrayC(initialData.B, initialData.A);

                for (int i = 0; i < initialData.dimension; i++)
                {
                    sw.Write($"{initialData.C[i]} ");
                }

                VmMainW.ListC = VmMainW.GetListFromArray(initialData.C);


                sw.WriteLine("\n\nМассив Y\n");

                sw.WriteLine("x \t Лагранж");

                initialData.Y = Arrays.MakeArrayY(initialData.gstep, initialData.precision, initialData.C);

                for (int arrayYIndex = 0; arrayYIndex < initialData.Y.GetLength(0); arrayYIndex++)
                {
                    sw.WriteLine($"{initialData.Y[arrayYIndex, 0]} \t {initialData.Y[arrayYIndex, 1]}");
                }

                VmMainW.ListY = VmMainW.GetListFromArray(initialData.Y);


                sw.WriteLine($"\nСортированный массив Y\n");
                sw.WriteLine("x \t Y");

                initialData.SortY = Arrays.Sort(initialData.Y);

                for (int arrayYIndex = 0; arrayYIndex < initialData.Y.GetLength(0); arrayYIndex++)
                {
                    sw.WriteLine($"{initialData.SortY[arrayYIndex, 0]} \t {initialData.SortY[arrayYIndex, 1]}");
                }

                VmMainW.ListSortY = VmMainW.GetListFromArray(initialData.SortY);
            };

            VmMainW.AxisXLabels = VmMainW.ListY.Select(r => r.x.ToString("N2")).ToArray(); //Массив для подписей к точкам на графике

            VmMainW.Series = new SeriesCollection // Коллекция для графика
            {
                new LineSeries
                {
                    Title = "Y",
                    Values = VmMainW.ListY.Select(r => r.y).AsChartValues(),
                },
                new LineSeries
                {
                    Title = "Сортированный Y",
                    Values = VmMainW.ListSortY.Select(r => r.y).AsChartValues(),
                }
            };


            VmMainW.LoadFileContent(filePath);
            MessageBox.Show("Файл заполнен");
            VmMainW.IsTabsEnable = true;
        }
    }
}
   
