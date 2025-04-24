using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class SeriesCalc
    {
        /// <summary>
        /// Вычисление суммы ряда с заданной точностью
        /// </summary>
        public static double SeriesSum(double x, double eps = 0.001)
        {
            double sum = 0;
            double currentElement = Math.Pow(2 * x, 2) / Factorial(2);
            int i = 1;
            int sign = 1;
            while (Math.Abs(currentElement) > eps)
            {
                sum += sign * currentElement;
                i++;
                sign *= -1;
                currentElement = Math.Pow(2 * x, 2 * i) / Factorial(2 * i);
            }
            return sum;
        }

        public static double ControlFormula(double x)
        {
            double ctrl = 2*Math.Pow(Math.Sin(x), 2);
            return ctrl;
        }

        private static double Factorial(double x)
        {
            if (x == 0)
            {
                return 1;
            }
            double res = 1;
            for (int i = 1; i <= x; i++)
            {
                res *= i;
            }
            return res;
        }
    }
}
