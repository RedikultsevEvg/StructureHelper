using NUnit.Framework;
using Moq;
using System.IO;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;

namespace StructureHelperTests.UnitTests.ForcesTests
{


    [TestFixture]
    public class CheckColumnedFilePropertyLogicTests
    {
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private Mock<IColumnedFileProperty> _mockFileProperty;
        private CheckColumnedFilePropertyLogic _logic;

        [SetUp]
        public void SetUp()
        {
            _mockTraceLogger = new Mock<IShiftTraceLogger>();
            _mockFileProperty = new Mock<IColumnedFileProperty>();

            _logic = new CheckColumnedFilePropertyLogic
            {
                TraceLogger = _mockTraceLogger.Object,
                Entity = _mockFileProperty.Object
            };
        }

        [Test]
        public void Check_ShouldReturnTrue_WhenFileExistsAndSkipRowsAreValid()
        {
            // Arrange
            _mockFileProperty.Setup(x => x.SkipRowBeforeHeaderCount).Returns(1);
            _mockFileProperty.Setup(x => x.SkipRowHeaderCount).Returns(1);
            _mockFileProperty.Setup(x => x.FilePath).Returns("tmp_valid_file.xlsx");
            File.Create("tmp_valid_file.xlsx").Dispose(); // Create a temporary file

            // Act
            var result = _logic.Check();

            // Assert
            Assert.That(result, Is.True);

            // Cleanup
            File.Delete("tmp_valid_file.xlsx");
        }

        [Test]
        public void Check_ShouldReturnFalse_WhenSkipRowsAreNegative()
        {
            // Arrange
            _mockFileProperty.Setup(x => x.SkipRowBeforeHeaderCount).Returns(-1);
            _mockFileProperty.Setup(x => x.SkipRowHeaderCount).Returns(-1);

            // Act
            var result = _logic.Check();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_logic.CheckResult, Does.Contain("skip row count must be greater or equal to 0"));
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>(), TraceLogStatuses.Error), Times.Once);
        }

        [Test]
        public void Check_ShouldReturnFalse_WhenFileDoesNotExist()
        {
            // Arrange
            _mockFileProperty.Setup(x => x.SkipRowBeforeHeaderCount).Returns(1);
            _mockFileProperty.Setup(x => x.SkipRowHeaderCount).Returns(1);
            _mockFileProperty.Setup(x => x.FilePath).Returns("non_existent_file.xlsx");

            // Act
            var result = _logic.Check();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_logic.CheckResult, Does.Contain("File non_existent_file.xlsx does not exist"));
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>(), TraceLogStatuses.Error), Times.Once);
        }
    }

}
