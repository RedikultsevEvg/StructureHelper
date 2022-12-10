using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Tests.Infrastructures.Logics;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System.Collections.Generic;
using System.Threading;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelper.Models.Materials;

namespace StructureHelperTests.FunctionalTests.Ndms.SteelSections
{
    public class ReinforcementTest
    {
        [TestCase(0.3, 0.6, 4e8, 0, 0, 1800000, 0d, 0d, 5e-5d)]
        [TestCase(0.3, 0.6, 4e8, 0, 0, -1800000, 0d, 0d, -5e-5d)]
        [TestCase(0.3, 0.6, 4e8, 7000000, 0, 0, 0.0065882684745345067d, 0d, 0d)]
        //[TestCase(0.3, 0.6, 6e8, 10000000, 0, 0, 0.010485801788961743d, 0d, -0.00011114996218404612d)]
        public void Run_ShouldPass(double width, double height, double strength, double mx, double my, double nz, double expectedKx, double expectedKy, double expectedEpsilonZ)
        {
            //Arrange
            var headMaterial = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforecement400, CodeTypes.EuroCode_2_1990);
            ITriangulationOptions options = new TriangulationOptions { LimiteState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm };
            INdmPrimitive primitive = new RectanglePrimitive { CenterX = 0, CenterY = 0, Width = width, Height = height, HeadMaterial = headMaterial, NdmMaxSize = 1, NdmMinDivision = 100 };
            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            primitives.Add(primitive);
            var ndmCollection = Triangulation.GetNdms(primitives, options);
            var loaderData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = 0.01,
                    MaxIterationCount = 100,
                    StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                },
                NdmCollection = ndmCollection
            };
            var calculator = new Calculator();
            //Act
            calculator.Run(loaderData, new CancellationToken());
            var results = calculator.Result;
            //Assert
            Assert.NotNull(results);
            var strainMatrix = results.StrainMatrix;
            Assert.NotNull(strainMatrix);
            Assert.AreEqual(expectedKx, strainMatrix.Kx, ExpectedProcessor.GetAccuracyForExpectedValue(expectedKx));
            Assert.AreEqual(expectedKy, strainMatrix.Ky, ExpectedProcessor.GetAccuracyForExpectedValue(expectedKy));
            Assert.AreEqual(expectedEpsilonZ, strainMatrix.EpsZ, ExpectedProcessor.GetAccuracyForExpectedValue(expectedEpsilonZ));
        }
    }
}
