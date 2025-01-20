using NUnit.Framework;
using Moq;
using System.Text;
using ExcelDataReader;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models;

namespace StructureHelperTests.UnitTests.ForcesTests
{


    [TestFixture]
    public class GetTupleByExcelReaderLogicTests
    {
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private Mock<IColumnedFileProperty> _mockFileProperty;
        private Mock<IExcelDataReader> _mockReader;
        private Mock<IFillTupleArrayByColumnNameLogic> _mockFillArray;
        private GetTupleByExcelReaderLogic _logic;

        [SetUp]
        public void SetUp()
        {
            _mockTraceLogger = new Mock<IShiftTraceLogger>();
            _mockFileProperty = new Mock<IColumnedFileProperty>();
            _mockReader = new Mock<IExcelDataReader>();
            _mockFillArray = new Mock<IFillTupleArrayByColumnNameLogic>();

            _logic = new GetTupleByExcelReaderLogic(_mockFillArray.Object)
            {
                TraceLogger = _mockTraceLogger.Object,
                ForceFileProperty = _mockFileProperty.Object
            };
        }

        [Test]
        public void GetForceTuple_ShouldReturnForceTuple_WhenDataIsValid()
        {
            // Arrange
            var columnProperties = new List<IColumnFileProperty>
        {
            new Mock<IColumnFileProperty>().SetupAllProperties().Object
        };

            _mockFileProperty.Setup(x => x.ColumnProperties).Returns(columnProperties);
            _mockFileProperty.Setup(x => x.GlobalFactor).Returns(1.0);
            _mockFillArray.Setup(x => x.ProceeForceTupleArray(It.IsAny<double[]>(), It.IsAny<double>(), It.IsAny<string>(),It.IsAny<string>()));

            _mockReader.Setup(x => x.GetValue(It.IsAny<int>())).Returns("1");

            // Act
            var result = _logic.GetForceTuple(_mockReader.Object);

            // Assert
            Assert.That(result, Is.InstanceOf<IForceTuple>());
        }
    }

}
