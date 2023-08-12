using LoaderCalculator.Tests.Infrastructures.Logics;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperTests.FunctionalTests.Ndms.Calculators.ForceCalculatorTests
{
    public class RCSectionsTest
    {
        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, false, -0.00062729176929923703d, -0.0029292919541166911d, 0.00035383082501577246d)]
        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 3, 2, false, -0.00046857734823565632d, -0.0025101896869558888d, 0.00027185795017719519d)]
        [TestCase(0.5d, 0.5d, 0.025d, 0.025d, 3, 3, false, -0.00081656737668168479d, -0.00081656737668168414d, 0.00011865117209051567d)]
        [TestCase(0.5d, 0.5d, 0.025d, 0.025d, 3, 3, true, -0.00081971887286298598d, -0.00081971887286298544d, 0.00011922273439756063d)]
        [TestCase(0.5d, 0.6d, 0.025d, 0.025d, 3, 3, true, -0.00048146233319312662d, -0.00077822315770951882d, 0.00010599549196849429d)]
        [TestCase(0.6d, 0.5d, 0.025d, 0.025d, 3, 3, true, -0.00077822315770951947d, -0.00048146233319312478d, 0.00010599549196849413d)]
        public void Run_ShouldPass(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, bool isBuckling, double expectedKx, double expectedKy, double expectedEpsZ)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount};
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.CalculatorsList[0] as IForceCalculator;
            calculator.CompressedMember.Buckling = isBuckling;
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

        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, false, true, true)]
        [TestCase(1d, 0.2d, 0.012d, 0.012d, 5, 2, false, true, false)]
        [TestCase(1d, 0.2d, 0.012d, 0.025d, 5, 2, false, true, true)]
        public void Run_ShouldPass_Result_IsNotValid(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount,bool isBuckling, bool calcResult, bool firstForceResult)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.CalculatorsList[0] as IForceCalculator;
            calculator.CompressedMember.Buckling = isBuckling;
            //Act
            calculator.Run();
            var result = calculator.Result as IForcesResults;
            //Assert
            Assert.IsNotNull(result);
            Assert.True(calcResult == result.IsValid);
            Assert.True(firstForceResult == result.ForcesResultList[0].IsValid);
        }

        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, 0d, 0d, 0d)]
        public void Run_ShouldPassPrestrain(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, double expectedKx, double expectedKy, double expectedEpsZ)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.CalculatorsList[0] as IForceCalculator;
            calculator.CompressedMember.Buckling = false;
            calculator.Run();
            var ndmPrimitives = newSection.SectionRepository.Primitives;
            var result = calculator.Result as IForcesResults;
            var strainMatrix = result.ForcesResultList[0].LoaderResults.StrainMatrix;
            var source = StrainTupleService.ConvertToStrainTuple(strainMatrix);
            //Act
            foreach (var item in ndmPrimitives)
            {
                ForceTupleService.CopyProperties(source, item.AutoPrestrain);
            }
            calculator.Run();
            var result2 = calculator.Result as IForcesResults;
            //Assert
            Assert.IsNotNull(result2);
            Assert.IsTrue(result2.IsValid);
            var strainMatrix2 = result2.ForcesResultList[0].LoaderResults.StrainMatrix;
            var kx = strainMatrix2.Kx;
            var ky = strainMatrix2.Ky;
            var epsz = strainMatrix2.EpsZ;
            Assert.AreEqual(expectedKx, kx, 1e-10);
            Assert.AreEqual(expectedKy, ky, 1e-10);
            Assert.AreEqual(expectedEpsZ, epsz, 1e-10);
        }
    }
}
