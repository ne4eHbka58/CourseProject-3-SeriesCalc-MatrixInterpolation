using Logic;

namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static bool doubleArrayEquals(double[,] arr, double[,] other)
        {
            if (arr.GetLength(0) != other.GetLength(0) ||
                arr.GetLength(1) != other.GetLength(1))
                return false;
            for (int i = 0; i < arr.GetLength(0); i++)
                for (int j = 0; j < arr.GetLength(1); j++)
                    if (arr[i, j].CompareTo(other[i, j]) != 0)
                        return false;
            return true;
        }

        [Test]
        public void TestControlFormula1()
        {
            double x = 0;
            double res = SeriesCalc.ControlFormula(x);
            double expectedRes = 0;
            Assert.That(expectedRes.Equals(res));
        }

        [Test]
        public void TestControlFormula2()
        {
            double x = 1;
            double res = Math.Round(SeriesCalc.ControlFormula(x), 9);
            double expectedRes = 1.416146837;
            Assert.That(expectedRes.Equals(res));
        }

        [Test]
        public void TestControlFormula3()
        {
            double x = 2;
            double res = Math.Round(SeriesCalc.ControlFormula(x), 9);
            double expectedRes = 1.653643621;
            Assert.That(expectedRes.Equals(res));
        }

        [Test]
        public void TestSeriesSum1()
        {
            double x = 0;
            double res = SeriesCalc.SeriesSum(x);
            double expectedRes = SeriesCalc.ControlFormula(x);
            Assert.That(expectedRes.Equals(res));
        }

        [Test]
        public void TestSeriesSum2()
        {
            double x = -5;
            double res = Math.Round(SeriesCalc.SeriesSum(x), 3);
            double expectedRes = Math.Round(SeriesCalc.ControlFormula(x), 3);
            Assert.That(expectedRes.Equals(res));
        }

        [Test]
        public void TestSeriesSum3()
        {
            double x = 5;
            double res = Math.Round(SeriesCalc.SeriesSum(x), 3);
            double expectedRes = Math.Round(SeriesCalc.ControlFormula(x), 3);
            Assert.That(expectedRes.Equals(res));
        }

        [Test]
        public void TestMakeArrayÑ()
        {
            double[,] Matrix1 = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            double[] Matrix2 = { 1, 2, 3 };
            double[] res = Arrays.MakeArrayC(Matrix1, Matrix2);
            double[] expectedRes = { 60, 72, 84 };
            Assert.That(expectedRes.SequenceEqual(res));
        }

        [Test]
        public void TestArraySort()
        {
            double[,] Matrix1 = { { 1, 10 }, { 2, 200 }, { 3, 30 } };
            double[,] Matrix2 = { { 1, 10 }, { 3, 30 }, { 2, 200 } };
            double[,] Matrix = Arrays.Sort(Matrix1);
            Assert.That(doubleArrayEquals(Matrix, Matrix2));
        }
    }
}