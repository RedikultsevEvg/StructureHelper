using LoaderCalculator.Data.Materials;
using Moq;
using NUnit.Framework;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Cracking.CheckLogics;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.MaterialTests
{
    [TestFixture]
    public class PrimitivesForCrackMaterialCheckLogicTests
    {
        private Mock<IShiftTraceLogger> traceLoggerMock;
        private PrimitivesForCrackMaterialCheckLogic logic;

        [SetUp]
        public void SetUp()
        {
            traceLoggerMock = new Mock<IShiftTraceLogger>();

            logic = new PrimitivesForCrackMaterialCheckLogic
            {
                TraceLogger = traceLoggerMock.Object
            };
        }

        [Test]
        public void Check_EntityIsNull_ReturnsFalseAndTraces()
        {
            // Arrange
            logic.Entity = null;

            // Act
            bool result = logic.Check();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(logic.CheckResult, Is.Not.Empty);

            traceLoggerMock.Verify(
                x => x.AddMessage(It.IsAny<string>()),
                Times.AtLeastOnce);
        }

        [Test]
        public void Check_NoCrackMaterial_ReturnsFalseAndTraces()
        {
            // Arrange
            logic.Entity = new[]
            {
            CreatePrimitive(new Mock<IMaterial>().Object),
            CreatePrimitive(new Mock<IMaterial>().Object)
        };

            // Act
            bool result = logic.Check();

            // Assert
            Assert.That(result, Is.False);
            Assert.That(logic.CheckResult, Does.Contain("supports cracking"));

            traceLoggerMock.Verify(
                x => x.AddMessage(It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public void Check_WithCrackMaterial_ReturnsTrueAndDoesNotTrace()
        {
            // Arrange
            logic.Entity = new[]
            {
            CreatePrimitive(new Mock<IMaterial>().Object),
            CreatePrimitive(new Mock<ICrackMaterial>().Object)
        };

            // Act
            bool result = logic.Check();

            // Assert
            Assert.That(result, Is.True);
            Assert.That(logic.CheckResult, Is.Empty);

            traceLoggerMock.Verify(
                x => x.AddMessage(It.IsAny<string>()),
                Times.Never);
        }

        // ---------- helper ----------

        private static INdmPrimitive CreatePrimitive(IMaterial material)
        {
            var headMaterialMock = new Mock<IHeadMaterial>();
            headMaterialMock
                .Setup(x => x.GetLoaderMaterial(LimitStates.SLS, CalcTerms.ShortTerm))
                .Returns(material);

            var ndmElementMock = new Mock<INdmElement>();
            ndmElementMock
                .SetupGet(x => x.HeadMaterial)
                .Returns(headMaterialMock.Object);

            var primitiveMock = new Mock<INdmPrimitive>();
            primitiveMock
                .SetupGet(x => x.NdmElement)
                .Returns(ndmElementMock.Object);

            return primitiveMock.Object;
        }
    }


}
