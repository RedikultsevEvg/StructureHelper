using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelperTests.UnitTests.ValueDiagramsTests
{
    [TestFixture]
    public class ValueDiagramEntityCheckLogicTests
    {
        private Mock<ICheckEntityLogic<IValueDiagram>> _diagramCheckMock;
        private Mock<IShiftTraceLogger> _loggerMock;
        private ValueDiagramEntityCheckLogic _logic;

        [SetUp]
        public void Setup()
        {
            _diagramCheckMock = new Mock<ICheckEntityLogic<IValueDiagram>>();
            _loggerMock = new Mock<IShiftTraceLogger>();
            _logic = new ValueDiagramEntityCheckLogic(_diagramCheckMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Check_EntityIsNull_ThrowsStructureHelperException()
        {
            // Arrange
            _logic.Entity = null;

            // Act + Assert
            Assert.That(
                () => _logic.Check(),
                Throws.Exception.TypeOf<StructureHelperException>()
            );
        }

        [Test]
        public void Check_DiagramCheckPasses_ReturnsTrue()
        {
            // Arrange
            var entity = new Mock<IValueDiagramEntity>();
            var diagram = new Mock<IValueDiagram>().Object;

            entity.Setup(e => e.ValueDiagram).Returns(diagram);
            entity.Setup(e => e.Name).Returns("TestDiagram");

            _logic.Entity = entity.Object;

            _diagramCheckMock.Setup(c => c.Check()).Returns(true);

            // Act
            var result = _logic.Check();

            // Assert
            Assert.That(result, Is.True);

            _diagramCheckMock.VerifySet(c => c.Entity = diagram, Times.Once);
            _diagramCheckMock.Verify(c => c.Check(), Times.Once);

            _loggerMock.Verify(l => l.AddMessage(It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Check_DiagramCheckFails_ReturnsFalse_AndLogsError()
        {
            // Arrange
            var entity = new Mock<IValueDiagramEntity>();
            var diagram = new Mock<IValueDiagram>().Object;

            entity.Setup(e => e.ValueDiagram).Returns(diagram);
            entity.Setup(e => e.Name).Returns("MyDiagram");

            _logic.Entity = entity.Object;

            _diagramCheckMock.Setup(c => c.Check()).Returns(false);
            _diagramCheckMock.Setup(c => c.CheckResult).Returns("child error message");

            // Act
            var result = _logic.Check();

            // Assert
            Assert.That(result, Is.False);

            _diagramCheckMock.Verify(c => c.Check(), Times.Once);

            _loggerMock.Verify(
                l => l.AddMessage(It.Is<string>(s => s.Contains("Diagram: MyDiagram has error: child error message"))),
                Times.Once
            );
        }

        [Test]
        public void Check_EntityNameIsNull_UsesUnnamedInMessage()
        {
            // Arrange
            var entity = new Mock<IValueDiagramEntity>();
            var diagram = new Mock<IValueDiagram>().Object;

            entity.Setup(e => e.ValueDiagram).Returns(diagram);
            entity.Setup(e => e.Name).Returns((string)null);

            _logic.Entity = entity.Object;

            _diagramCheckMock.Setup(c => c.Check()).Returns(false);
            _diagramCheckMock.Setup(c => c.CheckResult).Returns("xyz");

            // Act
            var result = _logic.Check();

            // Assert
            Assert.That(result, Is.False);

            _loggerMock.Verify(
                l => l.AddMessage(It.Is<string>(s => s.Contains("<unnamed>"))),
                Times.Once
            );
        }
    }
}
