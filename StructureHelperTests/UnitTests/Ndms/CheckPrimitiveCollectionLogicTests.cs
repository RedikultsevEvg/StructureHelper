using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;


namespace StructureHelperTests.UnitTests.Ndms
{
    [TestFixture]
    public class CheckPrimitiveCollectionLogicTests
    {
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private Mock<ICheckRebarPrimitiveLogic> _mockCheckRebarPrimitiveLogic;
        private Mock<IHasPrimitives> _mockHasPrimitives;
        private Mock<CheckPrimitiveCollectionLogic> _mockCheckPrimitiveCollectionLogic;

        [SetUp]
        public void SetUp()
        {
            _mockTraceLogger = new Mock<IShiftTraceLogger>();
            _mockCheckRebarPrimitiveLogic = new Mock<ICheckRebarPrimitiveLogic>();
            _mockHasPrimitives = new Mock<IHasPrimitives>();

            _mockCheckPrimitiveCollectionLogic = new Mock<CheckPrimitiveCollectionLogic>(_mockTraceLogger.Object, _mockCheckRebarPrimitiveLogic.Object)
            {
                CallBase = true
            };

            _mockCheckPrimitiveCollectionLogic.Object.HasPrimitives = _mockHasPrimitives.Object;
        }

        [Test]
        public void Check_PrimitivesIsNullOrEmpty_ReturnsFalseAndLogsError()
        {
            // Arrange
            _mockHasPrimitives.Setup(x => x.Primitives).Returns((List<INdmPrimitive>)null);

            // Act
            var result = _mockCheckPrimitiveCollectionLogic.Object.Check();

            // Assert
            Assert.IsFalse(result);
            Assert.That(_mockCheckPrimitiveCollectionLogic.Object.CheckResult, Is.EqualTo("Calculator does not contain any primitives\n"));
            //_mockTraceLogger.Verify(x => x.AddMessage("Calculator does not contain any primitives\n", TraceLogStatuses.Error), Times.Once);
        }

        [Test]
        public void Check_RebarPrimitiveFailsCheck_ReturnsFalseAndAppendsCheckResult()
        {
            // Arrange
            var rebarMock = new Mock<IRebarPrimitive>();
            _mockHasPrimitives.Setup(x => x.Primitives).Returns(new List<INdmPrimitive> { rebarMock.Object });
            _mockCheckRebarPrimitiveLogic.Setup(x => x.Check()).Returns(false);
            _mockCheckRebarPrimitiveLogic.Setup(x => x.CheckResult).Returns("Rebar check failed\n");

            // Act
            var result = _mockCheckPrimitiveCollectionLogic.Object.Check();

            // Assert
            Assert.IsFalse(result);
            Assert.That(_mockCheckPrimitiveCollectionLogic.Object.CheckResult, Is.EqualTo("Rebar check failed\n"));
        }

        [Test]
        public void Check_RebarPrimitiveHasNoHostPrimitive_ReturnsFalseAndLogsError()
        {
            // Arrange
            var rebarMock = new Mock<IRebarPrimitive>();
            var hostPrimitiveMock = new Mock<INdmPrimitive>();

            rebarMock.Setup(x => x.HostPrimitive).Returns(hostPrimitiveMock.Object);
            rebarMock.Setup(x => x.Name).Returns("RebarName");
            hostPrimitiveMock.Setup(x => x.Name).Returns("HostPrimitiveName");

            _mockHasPrimitives.Setup(x => x.Primitives).Returns(new List<INdmPrimitive> { rebarMock.Object });
            _mockCheckRebarPrimitiveLogic.Setup(x => x.Check()).Returns(true);  // Assume rebar check passes

            // Act
            var result = _mockCheckPrimitiveCollectionLogic.Object.Check();

            // Assert
            Assert.IsFalse(result);
            Assert.That(_mockCheckPrimitiveCollectionLogic.Object.CheckResult, Is.EqualTo("Host RebarName (HostPrimitiveName) is not included in primitives\n"));
            //_mockTraceLogger.Verify(x => x.AddMessage("Host RebarName (HostPrimitiveName) is not included in primitives\n", TraceLogStatuses.Error), Times.Once);
        }

        [Test]
        public void Check_AllPrimitivesValid_ReturnsTrue()
        {
            // Arrange
            var rebarMock = new Mock<IRebarPrimitive>();
            var hostPrimitiveMock = new Mock<INdmPrimitive>();

            rebarMock.Setup(x => x.HostPrimitive).Returns(hostPrimitiveMock.Object);
            rebarMock.Setup(x => x.Name).Returns("RebarName");

            _mockHasPrimitives.Setup(x => x.Primitives).Returns(new List<INdmPrimitive> { rebarMock.Object, hostPrimitiveMock.Object });
            _mockCheckRebarPrimitiveLogic.Setup(x => x.Check()).Returns(true);

            // Act
            var result = _mockCheckPrimitiveCollectionLogic.Object.Check();

            // Assert
            Assert.IsTrue(result);
            Assert.That(_mockCheckPrimitiveCollectionLogic.Object.CheckResult, Is.EqualTo(string.Empty));
        }
    }
}
