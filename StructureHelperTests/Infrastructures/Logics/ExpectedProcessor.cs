using System;
using System.Collections.Generic;
using System.Text;

namespace LoaderCalculator.Tests.Infrastructures.Logics
{
    internal static class ExpectedProcessor
    {
        internal static double GetAccuracyForExpectedValue(double expectedValue, double accuracy = 0.001d)
        {
            if (expectedValue == 0d) { return 1.0e-15d; }
            else return Math.Abs(expectedValue) * accuracy;
        }
    }
}
