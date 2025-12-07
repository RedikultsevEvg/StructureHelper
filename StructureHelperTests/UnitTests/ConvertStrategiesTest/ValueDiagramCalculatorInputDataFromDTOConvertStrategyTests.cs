using DataAccess.DTOs;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest
{


    [TestFixture]
    public class ValueDiagramCalculatorInputDataFromDTOConvertStrategyTests
    {
        private Mock<IUpdateStrategy<IValueDiagramCalculatorInputData>> mockUpdate;
        private Mock<IConvertStrategy<ValueDiagramEntity, ValueDiagramEntityDTO>> mockDiagramConvert;
        private Mock<IProcessLogic<IHasForcesAndPrimitives>> mockForcePrimLogic;
        private Mock<IConvertStrategy<Accuracy, AccuracyDTO>> mockAccuracyConvert;

        private ValueDiagramCalculatorInputDataFromDTOConvertStrategy sut;

        private ValueDiagramCalculatorInputDataDTO sampleDTO;
        private ValueDiagramEntityDTO diagramDTO;
        private ValueDiagramEntity sampleDiagramEntity;

        [SetUp]
        public void SetUp()
        {
            mockUpdate = new Mock<IUpdateStrategy<IValueDiagramCalculatorInputData>>(MockBehavior.Strict);
            mockDiagramConvert = new Mock<IConvertStrategy<ValueDiagramEntity, ValueDiagramEntityDTO>>(MockBehavior.Strict);
            mockForcePrimLogic = new Mock<IProcessLogic<IHasForcesAndPrimitives>>(MockBehavior.Strict);
            mockAccuracyConvert = new Mock<IConvertStrategy<Accuracy, AccuracyDTO>>(MockBehavior.Strict);

            sut = new ValueDiagramCalculatorInputDataFromDTOConvertStrategy(
                mockUpdate.Object,
                mockDiagramConvert.Object,
                mockForcePrimLogic.Object,
                mockAccuracyConvert.Object
            );

            diagramDTO = new ValueDiagramEntityDTO(Guid.Empty);
            sampleDiagramEntity = new ValueDiagramEntity(Guid.NewGuid());

            sampleDTO = new ValueDiagramCalculatorInputDataDTO(Guid.NewGuid())
            {
            };
            sampleDTO.Diagrams.Add(diagramDTO);
        }

        [Test]
        public void GetNewItem_ShouldReturnItemWithSameId()
        {
            // Arrange
            mockUpdate.Setup(x => x.Update(It.IsAny<IValueDiagramCalculatorInputData>(), sampleDTO));
            mockForcePrimLogic.SetupSet(x => x.Source = sampleDTO);
            mockForcePrimLogic.SetupSet(x => x.Target = It.IsAny<IValueDiagramCalculatorInputData>());
            mockForcePrimLogic.Setup(x => x.Process());
            mockDiagramConvert.Setup(x => x.Convert(diagramDTO)).Returns(sampleDiagramEntity);

            // Act
            var result = sut.GetNewItem(sampleDTO);

            // Assert
            Assert.That(sampleDTO.Id, Is.EqualTo(result.Id));
        }

        [Test]
        public void GetNewItem_ShouldCallUpdateStrategy()
        {
            // Arrange
            mockUpdate.Setup(x => x.Update(It.IsAny<IValueDiagramCalculatorInputData>(), sampleDTO));
            mockForcePrimLogic.SetupSet(x => x.Source = sampleDTO);
            mockForcePrimLogic.SetupSet(x => x.Target = It.IsAny<IValueDiagramCalculatorInputData>());
            mockForcePrimLogic.Setup(x => x.Process());
            mockDiagramConvert.Setup(x => x.Convert(diagramDTO)).Returns(sampleDiagramEntity);

            // Act
            sut.GetNewItem(sampleDTO);

            // Assert
            mockUpdate.Verify(x => x.Update(It.IsAny<IValueDiagramCalculatorInputData>(), sampleDTO), Times.Once);
        }

        [Test]
        public void GetNewItem_ShouldCallForcePrimitiveProcessing()
        {
            // Arrange
            mockUpdate.Setup(x => x.Update(It.IsAny<IValueDiagramCalculatorInputData>(), sampleDTO));

            mockForcePrimLogic.SetupSet(x => x.Source = sampleDTO);
            mockForcePrimLogic.SetupSet(x => x.Target = It.IsAny<IValueDiagramCalculatorInputData>());
            mockForcePrimLogic.Setup(x => x.Process());

            mockDiagramConvert.Setup(x => x.Convert(diagramDTO)).Returns(sampleDiagramEntity);

            // Act
            sut.GetNewItem(sampleDTO);

            // Assert
            mockForcePrimLogic.Verify(x => x.Process(), Times.Once);
        }

        [Test]
        public void GetNewItem_ShouldConvertAllDiagrams()
        {
            // Arrange
            mockUpdate.Setup(x => x.Update(It.IsAny<IValueDiagramCalculatorInputData>(), sampleDTO));

            mockForcePrimLogic.SetupSet(x => x.Source = sampleDTO);
            mockForcePrimLogic.SetupSet(x => x.Target = It.IsAny<IValueDiagramCalculatorInputData>());
            mockForcePrimLogic.Setup(x => x.Process());

            mockDiagramConvert.Setup(x => x.Convert(diagramDTO)).Returns(sampleDiagramEntity);

            // Act
            var result = sut.GetNewItem(sampleDTO);

            // Assert
            Assert.That(1, Is.EqualTo(result.Diagrams.Count));
            Assert.That(sampleDiagramEntity, Is.SameAs(result.Diagrams[0]));
        }
    }

}
