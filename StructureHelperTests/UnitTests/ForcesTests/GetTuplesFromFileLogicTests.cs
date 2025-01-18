using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperTests.UnitTests.ForcesTests
{


    [TestFixture]
    public class GetTuplesFromFileLogicTests
    {
        private Mock<IGetTupleByExcelReaderLogic> _mockExcelReaderLogic;
        private Mock<ICheckEntityLogic<IColumnedFileProperty>> _mockCheckEntityLogic;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private GetTuplesFromFileLogic _logic;

        [SetUp]
        public void SetUp()
        {
            _mockExcelReaderLogic = new Mock<IGetTupleByExcelReaderLogic>();
            _mockCheckEntityLogic = new Mock<ICheckEntityLogic<IColumnedFileProperty>>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();

            _logic = new GetTuplesFromFileLogic(
                _mockExcelReaderLogic.Object,
                _mockCheckEntityLogic.Object
            )
            {
                TraceLogger = _mockTraceLogger.Object
            };
        }

        [Test]
        public void GetTuples_ShouldReturnListOfForceTuples_WhenDataIsValid()
        {
            // Arrange
            var mockFileProperty = new Mock<IColumnedFileProperty>();
            mockFileProperty.Setup(x => x.FilePath).Returns("valid_file.xlsx");
            mockFileProperty.Setup(x => x.SkipRowBeforeHeaderCount).Returns(2);
            mockFileProperty.Setup(x => x.SkipRowHeaderCount).Returns(1);

            _logic.ForceFileProperty = mockFileProperty.Object;

            var mockForceTuple = new Mock<IForceTuple>();
            var forceTuples = new List<IForceTuple> { mockForceTuple.Object };

            _mockCheckEntityLogic.Setup(x => x.Check()).Returns(true);
            _mockExcelReaderLogic.Setup(x => x.GetForceTuple(It.IsAny<ExcelDataReader.IExcelDataReader>())).Returns(mockForceTuple.Object);

            // Mock file reading and ExcelReader behavior
            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("mock data"));
            FileStream mockStream = MockFileHelper.CreateMockFileStream(fileStream);

            // Act
            var result = _logic.GetTuples();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(forceTuples.Count));
            _mockCheckEntityLogic.Verify(x => x.Check(), Times.Once);
            _mockExcelReaderLogic.Verify(x => x.GetForceTuple(It.IsAny<ExcelDataReader.IExcelDataReader>()), Times.AtLeastOnce);
        }

        [Test]
        public void GetTuples_ShouldThrowException_WhenCheckFails()
        {
            // Arrange
            _mockCheckEntityLogic.Setup(x => x.Check()).Returns(false);
            _mockCheckEntityLogic.Setup(x => x.CheckResult).Returns("Invalid file property");

            // Act & Assert
            var ex = Assert.Throws<StructureHelperException>(() => _logic.GetTuples());
            Assert.That(ex.Message, Is.EqualTo("Invalid file property"));
        }

        [Test]
        public void GetTuples_ShouldLogErrorAndThrowException_WhenExceptionOccursInDataProcessing()
        {
            // Arrange
            var mockFileProperty = new Mock<IColumnedFileProperty>();
            mockFileProperty.Setup(x => x.FilePath).Returns("valid_file.xlsx");
            mockFileProperty.Setup(x => x.SkipRowBeforeHeaderCount).Returns(1);
            mockFileProperty.Setup(x => x.SkipRowHeaderCount).Returns(1);

            _logic.ForceFileProperty = mockFileProperty.Object;
            _mockCheckEntityLogic.Setup(x => x.Check()).Returns(true);

            _mockExcelReaderLogic.Setup(x => x.GetForceTuple(It.IsAny<ExcelDataReader.IExcelDataReader>()))
                .Throws(new StructureHelperException("Mock exception"));

            // Act & Assert
            var ex = Assert.Throws<StructureHelperException>(() => _logic.GetTuples());
            Assert.That(ex.Message, Does.Contain("Mock exception"));
            _mockTraceLogger.Verify(x => x.AddMessage(It.IsAny<string>(), TraceLogStatuses.Error), Times.AtLeastOnce);
        }

        public static class MockFileHelper
        {
            public static FileStream CreateMockFileStream(Stream inputStream)
            {
                var tempFilePath = Path.GetTempFileName();
                using (var fileStream = File.Create(tempFilePath))
                {
                    inputStream.CopyTo(fileStream);
                }
                return new FileStream(tempFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 4096, FileOptions.DeleteOnClose);
            }
        }

    }


}
