using LoaderCalculator;
using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Tests.Infrastructures.Logics;
using NUnit.Framework;
using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using StructureHelperLogics.NdmCalculations.Materials;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System.Collections.Generic;
using System.Threading;

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
            double width = 0.4;
            double height = 0.6;
            var ndmCollection = new List<INdm>();
            ndmCollection.AddRange(GetConcreteNdms(width, height));
            ndmCollection.AddRange(GetReinforcementNdms(width, height, topArea, bottomArea));
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

        private IEnumerable<INdm> GetConcreteNdms(double width, double height)
        {
            double strength = 40e6;
            ICenter center = new Center() { X = 0, Y = 0 };
            IRectangle rectangle = new Rectangle() { Width = width, Height = height, Angle = 0 };
            IPrimitiveMaterial material = new PrimitiveMaterial() { MaterialType = MaterialTypes.Concrete, ClassName = "С20", Strength = strength };
            ITriangulationOptions options = new TriangulationOptions() { LimiteState = StructureHelperLogics.Infrastructures.CommonEnums.LimitStates.Collapse, CalcTerm = StructureHelperLogics.Infrastructures.CommonEnums.CalcTerms.ShortTerm };
            INdmPrimitive primitive = new NdmPrimitive() { Center = center, Shape = rectangle, PrimitiveMaterial = material, NdmMaxSize = 1, NdmMinDivision = 20 };
            List<INdmPrimitive> primitives = new List<INdmPrimitive>();
            primitives.Add(primitive);
            var ndmCollection = Triangulation.GetNdms(primitives, options);
            return ndmCollection;
        }

        private IEnumerable<INdm> GetReinforcementNdms(double width, double height, double topArea, double bottomArea)
        {
            double gap = 0.05d;
            double strength = 4e8;
            IShape topReinforcement = new Point() { Area = topArea };
            IShape bottomReinforcement = new Point() { Area = bottomArea };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial() { MaterialType = MaterialTypes.Reinforcement, ClassName = "S400", Strength = strength };
            ITriangulationOptions options = new TriangulationOptions() { LimiteState = StructureHelperLogics.Infrastructures.CommonEnums.LimitStates.Collapse, CalcTerm = StructureHelperLogics.Infrastructures.CommonEnums.CalcTerms.ShortTerm };
            IMaterial material = Triangulation.GetMaterial(primitiveMaterial, options);
            ICenter centerRT = new Center() { X = width / 2 - gap, Y = height / 2 - gap };
            ICenter centerLT = new Center() { X = - (width / 2 - gap), Y = height / 2 - gap };
            ICenter centerRB = new Center() { X = width / 2 - gap, Y = - (height / 2 - gap) };
            ICenter centerLB = new Center() { X = -(width / 2 - gap), Y = - (height / 2 - gap) };
            IPointTriangulationLogicOptions optionsRT = new PointTriangulationLogicOptions(centerRT, topArea);
            IPointTriangulationLogicOptions optionsLT = new PointTriangulationLogicOptions(centerLT, topArea);
            IPointTriangulationLogicOptions optionsRB = new PointTriangulationLogicOptions(centerRB, bottomArea);
            IPointTriangulationLogicOptions optionsLB = new PointTriangulationLogicOptions(centerLB, bottomArea);
            var ndmCollection = new List<INdm>();
            ndmCollection.AddRange((new PointTriangulationLogic(optionsRT)).GetNdmCollection(material));
            ndmCollection.AddRange((new PointTriangulationLogic(optionsLT)).GetNdmCollection(material));
            ndmCollection.AddRange((new PointTriangulationLogic(optionsRB)).GetNdmCollection(material));
            ndmCollection.AddRange((new PointTriangulationLogic(optionsLB)).GetNdmCollection(material));
            return ndmCollection;
        }
    }
}
