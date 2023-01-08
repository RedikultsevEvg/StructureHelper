using LoaderCalculator.Tests.Infrastructures.Logics;
using NUnit.Framework;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperTests.FunctionalTests.Ndms.Calculators.ForceCalculatorTests
{
    public class RCSectionsTest
    {
        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, -0.00062544561815463693d, -0.0029292919541166911d, 0.00035383082501577246d)]
        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 3, 2, -0.00046762265275279838d, -0.0025101896869558888d, 0.00027185795017719519d)]
        [TestCase(0.5d, 0.5d, 0.025d, 0.025d, 3, 3, -0.00080914991212906239d, -0.00080914991212906184d, 0.00011900072665826425d)]
        public void Run_SouldPass(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, double expectedKx, double expectedKy, double expectedEpsZ)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount};
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.CalculatorsList[0];
            //Act
            calculator.Run();
            var result = calculator.Result as IForcesResults;
            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);
            var strainMatrix = result.ForcesResultList[0].LoaderResults.StrainMatrix;
            var kx = strainMatrix.Kx;
            var ky = strainMatrix.Ky;
            var epsz = strainMatrix.EpsZ;
            Assert.AreEqual(expectedKx, kx, ExpectedProcessor.GetAccuracyForExpectedValue(expectedKx));
            Assert.AreEqual(expectedKy, ky, ExpectedProcessor.GetAccuracyForExpectedValue(expectedKy));
            Assert.AreEqual(expectedEpsZ, epsz, ExpectedProcessor.GetAccuracyForExpectedValue(expectedEpsZ));
        }

        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, true, true)]
        [TestCase(1d, 0.2d, 0.012d, 0.012d, 5, 2, true, false)]
        [TestCase(1d, 0.2d, 0.012d, 0.025d, 5, 2, true, true)]
        public void Run_SouldPass_Result_IsNotValid(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, bool calcResult, bool firstForceResult)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.CalculatorsList[0];
            //Act
            calculator.Run();
            var result = calculator.Result as IForcesResults;
            //Assert
            Assert.IsNotNull(result);
            Assert.True(calcResult == result.IsValid);
            Assert.True(firstForceResult == result.ForcesResultList[0].IsValid);
        }
    }
}
