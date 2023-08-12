using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Tests.Infrastructures.Logics;
using NUnit.Framework;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System.Collections.Generic;
using System.Threading;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.Models.Primitives;
using StructureHelperCommon.Infrastructures.Settings;

namespace StructureHelperTests.FunctionalTests.Ndms.RCSections
{
    public class RCSectionTest
    {
        //Theoretical limit momemt Mx = 43kN*m
        [TestCase(0.000113, 0.000494, 10e3, 0d, 0d, 0.00084665917358052976d, 0.0d, 0.00020754144937701132d)]
        [TestCase(0.000113, 0.000494, 40e3, 0d, 0d, 0.0033939850380287412d, 0d, 0.00082989880025069202d)]
        [TestCase(0.000113, 0.000494, 42e3, 0d, 0d, 0.0056613831873867241d, 0d, 0.0014291081844183839d)]
        //Theoretical limit momemt Mx = -187kN*m
        [TestCase(0.000113, 0.000494, -50e3, 0d, 0d, -0.0011229555729294297d, 0d, 0.00021353225742956321d)]
        [TestCase(0.000113, 0.000494, -180e3, 0d, 0d, -0.0098365950945499738d, 0d, 0.0022035516889170013d)]
        [TestCase(0.000113, 0.000494, -183e3, 0d, 0d, -0.021718635290382458d, 0d, 0.0053526701372818789d)]
        public void Run_ShouldPass(double topArea, double bottomArea, double mx, double my, double nz, double expectedKx, double expectedKy, double expectedEpsilonZ)
        {
            //Arrange
            ProgramSetting.NatSystem = NatSystems.EU;
            double width = 0.4;
            double height = 0.6;
            var ndmCollection = new List<INdm>();
            ITriangulationOptions options = new TriangulationOptions { LimiteState = LimitStates.ULS, CalcTerm = CalcTerms.ShortTerm };
            var primitives = new List<INdmPrimitive>();
            primitives.AddRange(GetConcreteNdms(width, height));
            primitives.AddRange(GetReinforcementNdms(width, height, topArea, bottomArea));
            ndmCollection.AddRange(primitives.SelectMany(x => x.GetNdms(options)));
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

        private IEnumerable<INdmPrimitive> GetConcreteNdms(double width, double height)
        {
            
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Concrete40);
            var primitive = new RectanglePrimitive(material) {Width = width, Height = height, NdmMaxSize = 1, NdmMinDivision = 20 };
            List<INdmPrimitive> primitives = new List<INdmPrimitive> {primitive};
            return primitives;
        }

        private IEnumerable<INdmPrimitive> GetReinforcementNdms(double width, double height, double topArea, double bottomArea)
        {
            double gap = 0.05d;
            var material = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400);
            IPoint2D centerRT = new Point2D { X = width / 2 - gap, Y = height / 2 - gap };
            IPoint2D centerLT = new Point2D { X = - (width / 2 - gap), Y = height / 2 - gap };
            IPoint2D centerRB = new Point2D { X = width / 2 - gap, Y = - (height / 2 - gap) };
            IPoint2D centerLB = new Point2D { X = -(width / 2 - gap), Y = - (height / 2 - gap) };
            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            INdmPrimitive primitive;
            //Right top bar
            primitive = new PointPrimitive(material) { Area = topArea};
            primitive.Center.X = centerRT.X;
            primitive.Center.Y = centerRT.Y;
            primitives.Add(primitive);
            //Left top bar
            primitive = new PointPrimitive(material) { Area = topArea};
            primitive.Center.X = centerLT.X;
            primitive.Center.Y = centerLT.Y;
            primitives.Add(primitive);
            //Right bottom bar
            primitive = new PointPrimitive(material) { Area = bottomArea};
            primitive.Center.X = centerRB.X;
            primitive.Center.Y = centerRB.Y;
            primitives.Add(primitive);
            //Left bottom bar
            primitive = new PointPrimitive(material) {Area = bottomArea };
            primitive.Center.X = centerLB.X;
            primitive.Center.Y = centerLB.Y;
            primitives.Add(primitive);
            return primitives;
        }
    }
}
