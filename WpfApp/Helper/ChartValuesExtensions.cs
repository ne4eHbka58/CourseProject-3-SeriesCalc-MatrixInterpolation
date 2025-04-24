using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp.Helper
{
    /// <summary>
    /// Класс для корректной работы графика
    /// </summary>
    public static class ChartValuesExtensions
    {
        public static ChartValues<T> AsChartValues<T>(this IEnumerable<T> source)
        {
            return new ChartValues<T>(source);
        }
    }
}
