using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.CrossSections
{


    [TestFixture]
    public class RepositoryPrimitiveOperationTests
    {
        private Mock<ICrossSectionRepository> repositoryMock;
        private List<INdmPrimitive> repositoryPrimitives;

        private Mock<IForceCalculator> forceCalculatorMock;
        private Mock<ICrackCalculator> crackCalculatorMock;
        private Mock<IValueDiagramCalculator> valueDiagramCalculatorMock;
        private Mock<ICurvatureCalculator> curvatureCalculatorMock;
        private Mock<ILimitCurvesCalculator> limitCurvesCalculatorMock;

        private List<INdmPrimitive> forcePrimitives;
        private List<INdmPrimitive> crackPrimitives;
        private List<INdmPrimitive> valueDiagramPrimitives;
        private List<INdmPrimitive> curvaturePrimitives;

        private RepositoryPrimitiveOperation operation;

        [SetUp]
        public void SetUp()
        {
            repositoryPrimitives = new List<INdmPrimitive>
        {
            Mock.Of<INdmPrimitive>(),
            Mock.Of<INdmPrimitive>()
        };

            forcePrimitives = new List<INdmPrimitive>(repositoryPrimitives);
            crackPrimitives = new List<INdmPrimitive>(repositoryPrimitives);
            valueDiagramPrimitives = new List<INdmPrimitive>(repositoryPrimitives);
            curvaturePrimitives = new List<INdmPrimitive>(repositoryPrimitives);

            forceCalculatorMock = new Mock<IForceCalculator>();
            crackCalculatorMock = new Mock<ICrackCalculator>();
            valueDiagramCalculatorMock = new Mock<IValueDiagramCalculator>();
            curvatureCalculatorMock = new Mock<ICurvatureCalculator>();
            limitCurvesCalculatorMock = new Mock<ILimitCurvesCalculator>();

            repositoryMock = new Mock<ICrossSectionRepository>();
            repositoryMock.Setup(r => r.Primitives).Returns(repositoryPrimitives);
            repositoryMock.Setup(r => r.Calculators).Returns(new List<ICalculator>
            {
            forceCalculatorMock.Object,
            crackCalculatorMock.Object,
            valueDiagramCalculatorMock.Object,
            curvatureCalculatorMock.Object,
            limitCurvesCalculatorMock.Object
            });

            forceCalculatorMock.Setup(x => x.InputData.Primitives).Returns(forcePrimitives);
            crackCalculatorMock.Setup(x => x.InputData.Primitives).Returns(crackPrimitives);
            valueDiagramCalculatorMock.Setup(x => x.InputData.Primitives).Returns(valueDiagramPrimitives);
            curvatureCalculatorMock.Setup(x => x.InputData.Primitives).Returns(curvaturePrimitives);

            operation = new RepositoryPrimitiveOperation(repositoryMock.Object);
        }

        [Test]
        public void RemoveAll_ClearsPrimitivesInAllSupportedCalculators_AndRepository()
        {
            // Act
            operation.RemoveAll();

            // Assert
            Assert.That(forcePrimitives, Is.Empty);
            Assert.That(crackPrimitives, Is.Empty);
            Assert.That(valueDiagramPrimitives, Is.Empty);
            Assert.That(curvaturePrimitives, Is.Empty);
            Assert.That(repositoryPrimitives, Is.Empty);
        }

        [Test]
        public void Remove_RemovesPrimitiveFromAllCalculators_AndRepository()
        {
            // Arrange
            var primitive = repositoryPrimitives[0];

            // Act
            operation.Remove(primitive);

            // Assert
            Assert.That(forcePrimitives, Does.Not.Contain(primitive));
            Assert.That(crackPrimitives, Does.Not.Contain(primitive));
            Assert.That(valueDiagramPrimitives, Does.Not.Contain(primitive));
            Assert.That(curvaturePrimitives, Does.Not.Contain(primitive));
            Assert.That(repositoryPrimitives, Does.Not.Contain(primitive));
        }

        [Test]
        public void Remove_MultipleEntities_RemovesAll()
        {
            // Arrange
            var toRemove = new List<INdmPrimitive>(repositoryPrimitives);

            // Act
            operation.Remove(toRemove);

            // Assert
            Assert.That(forcePrimitives, Is.Empty);
            Assert.That(crackPrimitives, Is.Empty);
            Assert.That(valueDiagramPrimitives, Is.Empty);
            Assert.That(curvaturePrimitives, Is.Empty);
            Assert.That(repositoryPrimitives, Is.Empty);
        }

        [Test]
        public void RemoveAll_UnsupportedCalculatorType_ThrowsStructureHelperException()
        {
            // Arrange
            Mock<ICalculator> calculator = new Mock<ICalculator>();
            repositoryMock.Setup(r => r.Calculators)
                .Returns(new List<ICalculator>() { calculator.Object});

            // Act + Assert
            Assert.Throws<StructureHelperException>(() => operation.RemoveAll());
        }
    }

}
