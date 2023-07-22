using LoaderCalculator.Tests.Infrastructures.Logics;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace StructureHelperTests.FunctionalTests.Ndms.Calculators.CrackCalculatorTests

{
    internal class CrackCalculatorTest
    {
        [TestCase(0.4d, 0.6d, 0.012d, 0.025d, 2, 2, 0.81d)]
        public void Run_ShouldPass(double width, double height, double topDiametr, double bottomDiametr, int widthCount, int heightCount, double expectedFactor)
        {
            //Arrange
            var template = new RectangleBeamTemplate(width, height) { TopDiameter = topDiametr, BottomDiameter = bottomDiametr, WidthCount = widthCount, HeightCount = heightCount };
            var newSection = new SectionTemplate(new RectGeometryLogic(template)).GetCrossSection();
            var ndmPrimitives = newSection.SectionRepository.Primitives;
            ITriangulationOptions options = new TriangulationOptions { LimiteState = LimitStates.SLS, CalcTerm = CalcTerms.ShortTerm };
            var ndms = Triangulation.GetNdms(ndmPrimitives, options);
            var calculator = new CrackForceCalculator();
            calculator.EndTuple = new ForceTuple() { Mx = -50e3d, My = -50e3d, Nz = 0d };
            calculator.NdmCollection = ndms;
            //Act
            calculator.Run();
            var result = (CrackForceResult)calculator.Result;
            //Assert
            Assert.NotNull(result);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(expectedFactor, result.FactorOfCrackAppearance, 0.01d);
        }
    }
}
