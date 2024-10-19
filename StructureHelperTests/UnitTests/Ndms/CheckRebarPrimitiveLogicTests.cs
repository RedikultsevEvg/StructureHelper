using LoaderCalculator.Data.Materials;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Models.Shapes;
using StructureHelper.Models.Materials;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperTests.UnitTests.Ndms
{
    [TestFixture]
    public class CheckRebarPrimitiveLogicTests
    {
        private Mock<IRebarNdmPrimitive> _mockRebarPrimitive;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private CheckRebarPrimitiveLogic _checkRebarPrimitiveLogic;

        [SetUp]
        public void SetUp()
        {
            _mockRebarPrimitive = new Mock<IRebarNdmPrimitive>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();

            _checkRebarPrimitiveLogic = new CheckRebarPrimitiveLogic(_mockTraceLogger.Object)
            {
                RebarPrimitive = _mockRebarPrimitive.Object
            };
        }

        [Test]
        public void Check_WhenHostPrimitiveIsNull_ReturnsFalseAndLogsError()
        {
            // Arrange
            _mockRebarPrimitive.Setup(x => x.HostPrimitive).Returns((INdmPrimitive)null);
            _mockRebarPrimitive.Setup(x => x.Name).Returns("RebarName");

            // Act
            var result = _checkRebarPrimitiveLogic.Check();

            // Assert
            Assert.IsFalse(result);
            Assert.That(_checkRebarPrimitiveLogic.CheckResult, Is.EqualTo("Primitive RebarName does not have a host\n"));
            //_mockTraceLogger.Verify(x => x.AddMessage("Primitive RebarName does not have a host\n", TraceLogStatuses.Error), Times.Once);
        }

        //[Test]
        //public void Check_WhenRebarPrimitiveIsOutOfHost_ReturnsFalseAndLogsError()
        //{
        //    // Arrange
        //    var mockHostPrimitive = new Mock<IHasDivisionSize>();
        //    mockHostPrimitive.Setup(x => x.IsPointInside(It.IsAny<IPoint2D>())).Returns(false);

        //    // Act
        //    var result = _checkRebarPrimitiveLogic.Check();

        //    // Assert
        //    Assert.IsFalse(result);
        //    Assert.That(_checkRebarPrimitiveLogic.CheckResult, Is.EqualTo("Primitive of rebar RebarName is out of its host HostName"));
        //    //_mockTraceLogger.Verify(x => x.AddMessage("Primitive of rebar RebarName is out of its host HostName", TraceLogStatuses.Error), Times.Once);
        //}

        [Test]
        public void Check_WhenHostMaterialDoesNotSupportCracking_ReturnsFalseAndLogsError()
        {
            // Arrange
            var mockHostPrimitive = new Mock<INdmPrimitive>();
            var mockHeadMaterial = new Mock<IHeadMaterial>();
            var mockHelperMaterial = new Mock<IHelperMaterial>();

            _mockRebarPrimitive.Setup(x => x.HostPrimitive).Returns(mockHostPrimitive.Object);
            _mockRebarPrimitive.Setup(x => x.Name).Returns("RebarName");
            mockHostPrimitive.Setup(x => x.NdmElement.HeadMaterial).Returns(mockHeadMaterial.Object);
            mockHeadMaterial.Setup(x => x.HelperMaterial).Returns(mockHelperMaterial.Object);

            // Act
            var result = _checkRebarPrimitiveLogic.Check();

            // Assert
            Assert.IsFalse(result);
            Assert.That(_checkRebarPrimitiveLogic.CheckResult, Is.EqualTo("Material of host of RebarName ()  does not support cracking\n"));
            //_mockTraceLogger.Verify(x => x.AddMessage("Material of host of RebarName ()  does not support cracking\n", TraceLogStatuses.Error), Times.Once);
        }

        //[Test]
        //public void Check_WhenAllConditionsAreMet_ReturnsTrue()
        //{
        //    // Arrange
        //    var mockHostPrimitive = new Mock<IHasDivisionSize>();
        //    var mockHeadMaterial = new Mock<IHeadMaterial>();
        //    var mockHelperMaterial = new Mock<ICrackedMaterial>();

        //    _mockRebarPrimitive.Setup(x => x.HostPrimitive).Returns(mockHostPrimitive.Object);
        //    _mockRebarPrimitive.Setup(x => x.Name).Returns("RebarName");
        //    mockHostPrimitive.Setup(x => x.IsPointInside(It.IsAny<IPoint2D>())).Returns(true);
        //    mockHostPrimitive.Setup(x => x.HeadMaterial).Returns(mockHeadMaterial.Object);
        //    mockHeadMaterial.Setup(x => x.HelperMaterial).Returns(mockHelperMaterial.Object);

        //    // Act
        //    var result = _checkRebarPrimitiveLogic.Check();

        //    // Assert
        //    Assert.IsTrue(result);
        //    Assert.That(_checkRebarPrimitiveLogic.CheckResult, Is.EqualTo(string.Empty));
        //}
    }

}
