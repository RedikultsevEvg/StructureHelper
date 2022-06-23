using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Ndms.Transformations;
using LoaderCalculator.Data.Planes;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator.Infrastructure;
using LoaderCalculator.Tests.Infrastructures.Logics;
using LoaderCalculator.Triangulations;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, 0.001d, 0d, 0.0d, -4.410e-05d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, 0.0015d, 0d, 0.0d, -6.666e-05d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, 0.002d, 0d, 0.0d, -8.131e-05d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, 0.003d, 0d, 0.0d, -8.131e-05d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, -0.001d, 0d, 0.0d, 0.001d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, 0d, 0d, -0.002d, 0d, 0.0d, 0.002d)]
        //Curvature prestrain only
        [TestCase(0.000494, 0.000494, 0d, 0d, 0d, -1e-5d, 0d, 0d, 5.4638e-6d, 0.0d, 1.069e-06d)]
        //Test shows that prestrain and external forces are neglegiate one by another
        [TestCase(0.000494, 0.000494, 0d, 0d, 3.952e5d, 0d, 0d, 0.001d, 0d, 0.0d, 0d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 6.873e5d, 0d, 0d, 0.002d, 0d, 0.0d, -1.703e-8d)]
        [TestCase(0.000494, 0.000494, 0d, 0d, 6.873e5d, 0d, 0d, 0.003d, 0d, 0.0d, -3.796e-8d)]
        public void Run_ShouldPassPrestrain(double topArea, double bottomArea, double mx, double my, double nz, double prestrainKx, double prestrainKy, double prestrainEpsZ, double expectedKx, double expectedKy, double expectedEpsilonZ)
        {
            //Arrange
            double width = 0.4;
            double height = 0.6;
            ArrangeMaterial(LimitStates.Collapse, true);
            List<INdm> ndmList = new List<INdm>();
            IStrainMatrix prestrainMatrix = new StrainMatrix() { Kx = prestrainKx, Ky = prestrainKy, EpsZ = prestrainEpsZ };
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
