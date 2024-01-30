using LoaderCalculator.Tests.Infrastructures.Logics;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Buckling;

namespace StructureHelperTests.FunctionalTests.Ndms.RCSections
{
    public class BucklingLogicTest
    {
        [TestCase(0d, 0d, 0d, 0d, 0d, -100d, -0.15d, 0.15d, 1d)]
        [TestCase(0d, 0d, -50d, 0d, 0d, -100d, -0.15d, 0.15d, 1.5d)]
        [TestCase(0d, 0d, -100d, 0d, 0d, -100d, -0.15d, 0.15d, 2d)]
        [TestCase(0d, 0d, 0d, -10d, 0d, -100d, -0.15d, 0.15d, 1d)]
        [TestCase(-5d, 0d, -50d, -10d, 0d, -100d, -0.15d, 0.15d, 1.5d)]
        [TestCase(-10d, 0d, -100d, -10d, 0d, -100d, -0.15d, 0.15d, 2d)]
        [TestCase(0d, 0d, 0d, -10d, 10d, -100d, -0.15d, 0.15d, 1d)]
        [TestCase(-5d, 5d, -50d, -10d, 10d, -100d, -0.15d, 0.15d, 1.5d)]
        [TestCase(-10d, 10d, -100d, -10d, 10d, -100d, -0.15d, 0.15d, 2d)]
        public void Run_ShoulPass_PhiLogicSP63(double longMx, double longMy, double longNz, double shortMx, double shortMy, double shortNz, double pointX, double pointY, double expectedPhi)
        {
            //Arrange
            var fullTuple = new ForceTuple() { Mx = shortMx, My = shortMy, Nz = shortNz };
            var longTuple = new ForceTuple() { Mx = longMx, My = longMy, Nz = longNz };
            var point = new Point2D() { X = pointX, Y = pointY };
            //Act
            var phiLogic = new PhiLogicSP63(fullTuple, longTuple, point);
            var phiL = phiLogic.GetPhil();
            //Assert
            Assert.AreEqual(expectedPhi, phiL, 0.01d);
        }

        [TestCase(0d, 0.5d, 0.15d)]
        [TestCase(0.1d, 0.5d, 0.2d)]
        [TestCase(1d, 0.5d, 1.5d)]
        public void Run_ShoulPass_DeltaELogicSP63(double eccentricity, double size, double expectedDeltaE)
        {
            //Arrange
            //Act
            var deltaELogic = new DeltaELogicSP63(eccentricity, size);
            var deltaE = deltaELogic.GetDeltaE();
            //Assert
            Assert.AreEqual(expectedDeltaE, deltaE, 0.01d);
        }
    }
}
