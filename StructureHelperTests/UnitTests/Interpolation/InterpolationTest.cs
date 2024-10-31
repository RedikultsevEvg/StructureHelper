using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.UnitTests.Interpolation
{
    public class InterpolationTest
    {
        [TestCase(1.5, 5.5, 1, 2, 5, 6)]
        public void InterpolationTestMethod(double x, double y, double x1, double x2, double y1, double y2)
        {
            //Arrange
            //Act
            var result = StructureHelperCommon.Services.MathUtils.Interpolation(x, x1, x2, y1, y2);
            //Assert
            Assert.AreEqual(result, y);

        }
    }
}
