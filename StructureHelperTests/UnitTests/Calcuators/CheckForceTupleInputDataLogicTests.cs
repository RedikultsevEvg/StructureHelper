using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperTests.UnitTests.Calcuators
{


    [TestFixture]
    public class CheckForceTupleInputDataLogicTests
    {
        private Mock<IForceTupleInputData> _mockInputData;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private CheckForceTupleInputDataLogic _checkLogic;

        [SetUp]
        public void SetUp()
        {
            _mockInputData = new Mock<IForceTupleInputData>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();

            _checkLogic = new CheckForceTupleInputDataLogic(_mockInputData.Object, _mockTraceLogger.Object);
        }

        [Test]
        public void Check_InputDataIsNull_ThrowsStructureHelperException()
        {
            // Arrange
            _checkLogic = new CheckForceTupleInputDataLogic(null, _mockTraceLogger.Object);

            // Act & Assert
            var ex = Assert.Throws<StructureHelperException>(() => _checkLogic.Check());
            Assert.That(ex.Message, Is.EqualTo(ErrorStrings.ParameterIsNull + ": Input data"));
            _mockTraceLogger.Verify(x => x.AddMessage(ErrorStrings.ParameterIsNull + ": Input data"), Times.Once);
        }

        [Test]
        public void Check_NdmCollectionIsNullOrEmpty_ReturnsFalseAndLogsError()
        {
            // Arrange
            _mockInputData.Setup(x => x.NdmCollection).Returns((IEnumerable<INdm>)null);

            // Act
            var result = _checkLogic.Check();

            // Assert
            Assert.IsFalse(result);
            //Assert.That(_checkLogic.CheckResult, Is.EqualTo("Ndm collection is null or empty"));
            _mockTraceLogger.Verify(x => x.AddMessage("\nNdm collection is null or empty"), Times.Once);
        }

        [Test]
        public void Check_TupleIsNull_ReturnsFalseAndLogsError()
        {
            // Arrange
            _mockInputData.Setup(x => x.NdmCollection).Returns(new List<INdm>());
            _mockInputData.Setup(x => x.ForceTuple).Returns((IForceTuple)null);

            // Act
            var result = _checkLogic.Check();

            // Assert
            Assert.IsFalse(result);
            //Assert.That(_checkLogic.CheckResult, Is.EqualTo("Force tuple is null"));
            _mockTraceLogger.Verify(x => x.AddMessage("\nForce tuple is null"), Times.Once);
        }

        [Test]
        public void Check_AccuracyIsNull_ReturnsFalseAndLogsError()
        {
            // Arrange
            _mockInputData.Setup(x => x.NdmCollection).Returns(new List<INdm>());
            _mockInputData.Setup(x => x.ForceTuple).Returns(new ForceTuple());
            _mockInputData.Setup(x => x.Accuracy).Returns((IAccuracy)null);

            // Act
            var result = _checkLogic.Check();

            // Assert
            Assert.IsFalse(result);
            //Assert.That(_checkLogic.CheckResult, Is.EqualTo("Accuracy requirements is not assigned"));
            _mockTraceLogger.Verify(x => x.AddMessage("\nAccuracy requirements is not assigned"), Times.Once);
        }

        [Test]
        public void Check_AccuracyValuesAreInvalid_ReturnsFalseAndLogsErrors()
        {
            // Arrange
            var mockAccuracy = new Mock<IAccuracy>();
            mockAccuracy.Setup(x => x.IterationAccuracy).Returns(0);
            mockAccuracy.Setup(x => x.MaxIterationCount).Returns(0);

            _mockInputData.Setup(x => x.NdmCollection).Returns(new List<INdm>());
            _mockInputData.Setup(x => x.ForceTuple).Returns(new ForceTuple());
            _mockInputData.Setup(x => x.Accuracy).Returns(mockAccuracy.Object);

            // Act
            var result = _checkLogic.Check();

            // Assert
            Assert.IsFalse(result);
            //Assert.That(_checkLogic.CheckResult, Is.EqualTo("Value of accuracy 0 must be grater than zeroMax number of iteration 0 must be grater than zero"));
            _mockTraceLogger.Verify(x => x.AddMessage("\nValue of accuracy 0 must be grater than zero"), Times.Once);
            _mockTraceLogger.Verify(x => x.AddMessage("\nMax number of iteration 0 must be grater than zero"), Times.Once);
        }

        [Test]
        public void Check_AllConditionsMet_ReturnsTrue()
        {
            // Arrange
            var mockAccuracy = new Mock<IAccuracy>();
            mockAccuracy.Setup(x => x.IterationAccuracy).Returns(1);
            mockAccuracy.Setup(x => x.MaxIterationCount).Returns(10);

            _mockInputData.Setup(x => x.NdmCollection).Returns(new List<INdm> { new Mock<INdm>().Object });
            _mockInputData.Setup(x => x.ForceTuple).Returns(new ForceTuple());
            _mockInputData.Setup(x => x.Accuracy).Returns(mockAccuracy.Object);

            // Act
            var result = _checkLogic.Check();

            // Assert
            Assert.IsTrue(result);
            Assert.That(_checkLogic.CheckResult, Is.EqualTo(string.Empty));
        }
    }

}
