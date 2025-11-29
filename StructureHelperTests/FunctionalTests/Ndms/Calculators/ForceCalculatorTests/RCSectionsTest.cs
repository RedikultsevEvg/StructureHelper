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
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, false, -0.00068593466107645337d, -0.0030411189808055242d, 0.00034289928716916179d)]
        public void Run_ShouldPass(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, bool isBuckling, double expectedKx, double expectedKy, double expectedEpsZ)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height)
            {
                TopDiameter = topDiametr,
                BottomDiameter = bottomDiametr,
                WidthCount = widthCount,
                HeightCount = heightCount
            };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.Calculators[0] as ForceCalculator;
            calculator.InputData.CompressedMember.Buckling = isBuckling;
            //Act
            calculator.Run();
            var result = calculator.Result as IForceCalculatorResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsValid, Is.True);
            var strainMatrix = result.ForcesResultList[0].ForcesTupleResult.LoaderResults.StrainMatrix;
            var kx = strainMatrix.Kx;
            var ky = strainMatrix.Ky;
            var epsz = strainMatrix.EpsZ;
            Assert.That(kx, Is.EqualTo(expectedKx).Within(ExpectedProcessor.GetAccuracyForExpectedValue(expectedKx)));
            Assert.That(ky, Is.EqualTo(expectedKy).Within(ExpectedProcessor.GetAccuracyForExpectedValue(expectedKy)));
            Assert.That(epsz, Is.EqualTo(expectedEpsZ).Within(ExpectedProcessor.GetAccuracyForExpectedValue(expectedEpsZ)));
        }

        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, false, true, true)]
        [TestCase(1d, 0.2d, 0.012d, 0.012d, 5, 2, false, true, false)]
        [TestCase(1d, 0.2d, 0.012d, 0.025d, 5, 2, false, true, true)]
        public void Run_ShouldPass_Result_IsNotValid(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount,bool isBuckling, bool calcResult, bool firstForceResult)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.Calculators[0] as ForceCalculator;
            calculator.InputData.CompressedMember.Buckling = isBuckling;
            //Act
            calculator.Run();
            var result = calculator.Result as IForceCalculatorResult;
            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(calcResult == result.IsValid, Is.True);
            Assert.That(firstForceResult == result.ForcesResultList[0].IsValid, Is.True);
        }

        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, 0d, 0d, 0d)]
        public void Run_ShouldPassPrestrain(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, double expectedKx, double expectedKy, double expectedEpsZ)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var calculator = newSection.SectionRepository.Calculators[0] as ForceCalculator;
            calculator.InputData.CompressedMember.Buckling = false;
            calculator.Run();
            var ndmPrimitives = newSection.SectionRepository.Primitives;
            var result = calculator.Result as IForceCalculatorResult;
            var strainMatrix = result.ForcesResultList[0].ForcesTupleResult.LoaderResults.StrainMatrix;
            var source = ForceTupleConverter.ConvertToStrainTuple(strainMatrix);
            //Act
            foreach (var item in ndmPrimitives)
            {
                ForceTupleServiceLogic.CopyProperties(source, item.NdmElement.AutoPrestrain);
            }
            calculator.Run();
            var result2 = calculator.Result as IForceCalculatorResult;
            //Assert
            Assert.That(result2, Is.Not.Null);
            Assert.That(result2.IsValid, Is.True);
            var strainMatrix2 = result2.ForcesResultList[0].ForcesTupleResult.LoaderResults.StrainMatrix;
            var kx = strainMatrix2.Kx;
            var ky = strainMatrix2.Ky;
            var epsz = strainMatrix2.EpsZ;
            Assert.That(kx, Is.EqualTo(expectedKx).Within(1e-10));
            Assert.That(ky, Is.EqualTo(expectedKy).Within(1e-10));
            Assert.That(epsz, Is.EqualTo(expectedEpsZ).Within(1e-10));
        }
    }
}
