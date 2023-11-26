using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Ndms.Transformations;
using LoaderCalculator.Data.Planes;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Tests.Infrastructures.Logics;
using LoaderCalculator.Triangulations;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
namespace LoaderCalculator.Tests.FunctionalTests.SectionTests
{
    public class RCSectionTest
    {
        private IMaterialOptions _reinforcementOptions;
        private IMaterialBuilder _reinforcementBuilder;
        private IBuilderDirector _reinforcementDirector;
        private IMaterialOptions _concreteOptions;
        private IMaterialBuilder _concreteBuilder;
        private IBuilderDirector _concreteDirector;
        [SetUp]
        public void Setup()
        {
            _reinforcementOptions = new ReinforcementOptions();
            _reinforcementBuilder = new ReinforcementBuilder(_reinforcementOptions);
            _reinforcementDirector = new BuilderDirector(_reinforcementBuilder);
            _concreteOptions = new ConcreteOptions();
            _concreteBuilder = new ConcreteBuilder(_concreteOptions);
            _concreteDirector = new BuilderDirector(_concreteBuilder);
        }
        //Theoretical limit momemt Mx = 43kN*m
        [TestCase(0.000113, 0.000494, 10e3, 0d, 0d, 0.00086859958095297091d, 0.0d, 0.00020547463647966653d)]
        //Theoretical limit momemt Mx = -187kN*m
        [TestCase(0.000113, 0.000494, -50e3, 0d, 0d, -0.0011886813170062767d, 0d, 0.00020300578582187044d)]
        public void Run_ShouldPass(double topArea, double bottomArea, double mx, double my, double nz, double expectedKx, double expectedKy, double expectedEpsilonZ)
        {
            //Arrange
            double width = 0.4;
            double height = 0.6;
            ArrangeMaterial(LimitStates.Collapse, true);
            List<INdm> ndmList = new List<INdm>();
            ndmList.AddRange(GetConcreteNdms(width, height));
            ndmList.AddRange(GetReinforcementNdms(width, height, topArea, bottomArea));
            var loaderData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = 0.01,
                    MaxIterationCount = 100,
                    StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                },
                NdmCollection = ndmList
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
        //Longitudenal prestrain only
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, 0d, 0d, 0.0d, 0d)]
        //Curvature prestrain only
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, -1e-5d, 0d, 0d, 5.7463904747165557E-06d, 0.0d, 1.0286547851301493E-06d)]
        //Test shows that prestrain and external forces are neglegiate one by another
        [TestCase(0.000494, 0.000494, 0d, 0d, 3.952e5d, 0d, 0d, 0.001d, 0d, 0.0d, 0d)]
        public void Run_ShouldPassPrestrain(double topArea, double bottomArea, double mx, double my, double nz, double prestrainKx, double prestrainKy, double prestrainEpsZ, double expectedKx, double expectedKy, double expectedEpsilonZ)
        {
            //Arrange
            double width = 0.4;
            double height = 0.6;
            ArrangeMaterial(LimitStates.Collapse, true);
            List<INdm> ndmList = new List<INdm>();
            IStrainMatrix prestrainMatrix = new StrainMatrix { Kx = prestrainKx, Ky = prestrainKy, EpsZ = prestrainEpsZ };
            ndmList.AddRange(GetConcreteNdms(width, height));
            var reinforcement = GetReinforcementNdms(width, height, topArea, bottomArea);
            NdmTransform.SetPrestrain(reinforcement, prestrainMatrix);
            ndmList.AddRange(reinforcement);
            var loaderData = new LoaderOptions
            {
                Preconditions = new Preconditions
                {
                    ConditionRate = 0.001,
                    MaxIterationCount = 100,
                    StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                },
                NdmCollection = ndmList
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

        private void ArrangeMaterial(LimitStates limitStates, bool isShortTerm)
        {
            _reinforcementOptions.InitModulus = 2.0e11;
            _reinforcementOptions.LimitState = limitStates;
            _reinforcementOptions.IsShortTerm = isShortTerm;
            _reinforcementOptions.Strength = 400.0e6;

            _concreteOptions.LimitState = limitStates;
            _concreteOptions.IsShortTerm = isShortTerm;
            _concreteOptions.Strength = 40.0e6;
        }

        private IEnumerable<INdm> GetConcreteNdms(double width, double heigth)
        {
            IMaterial _material = _concreteDirector.BuildMaterial();
            RectangleTriangulationLogicOptions logicOptions = new RectangleTriangulationLogicOptions(width, heigth, 1, 20);
            var logic = Triangulation.GetLogicInstance(logicOptions);
            var ndmCollection = logic.GetNdmCollection(new RectangularPlane { Material = _material });
            return ndmCollection;
        }

        private IEnumerable<INdm> GetReinforcementNdms(double width, double heigth, double topArea, double bottomArea)
        {
            IMaterial _material = _reinforcementDirector.BuildMaterial();
            double gap = 0.05;
            var ndmCollection = new INdm[]
            {
                new Ndm
                {
                    Area = topArea,
                    CenterX = width / 2 - gap,
                    CenterY = heigth / 2 - gap,
                    Material = _material
                },
                new Ndm
                {
                    Area = topArea,
                    CenterX = -(width / 2 - gap),
                    CenterY = heigth / 2 - gap,
                    Material = _material
                },
                new Ndm
                {
                    Area = bottomArea,
                    CenterX = width / 2 - gap,
                    CenterY = -(heigth / 2 - gap),
                    Material = _material
                },
                new Ndm
                {
                    Area = bottomArea,
                    CenterX = -(width / 2 - gap),
                    CenterY = -(heigth / 2 - gap),
                    Material = _material
                }
            };
            return ndmCollection;
        }
    }
}
