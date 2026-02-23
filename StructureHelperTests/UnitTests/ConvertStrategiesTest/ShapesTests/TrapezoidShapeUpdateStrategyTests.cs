using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    [TestFixture]
    public class TrapezoidShapeUpdateStrategyTests
    {
        private TrapezoidShapeUpdateStrategy sut;

        [SetUp]
        public void SetUp()
        {
            sut = new TrapezoidShapeUpdateStrategy();
        }

        [Test]
        public void Update_WhenSourceIsNull_Throws()
        {
            var target = new Mock<ITrapezoidShape>().Object;

            Assert.Throws<StructureHelperException>(() => sut.Update(target, null));
            // Replace Exception with StructureHelperException if needed
        }

        [Test]
        public void Update_WhenTargetIsNull_Throws()
        {
            var source = new Mock<ITrapezoidShape>().Object;

            Assert.Throws<StructureHelperException>(() => sut.Update(null, source));
        }

        [Test]
        public void Update_WhenTargetEqualsSource_DoesNothing()
        {
            var shapeMock = new Mock<ITrapezoidShape>();

            sut.Update(shapeMock.Object, shapeMock.Object);

            shapeMock.VerifySet(x => x.Height = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.TopBase = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.BottomBase = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.TopBaseOffset = It.IsAny<double>(), Times.Never);
        }

        [Test]
        public void Update_WhenValidObjects_CopiesAllProperties()
        {
            // Arrange
            var sourceMock = new Mock<ITrapezoidShape>();
            sourceMock.SetupGet(x => x.Height).Returns(100);
            sourceMock.SetupGet(x => x.TopBase).Returns(40);
            sourceMock.SetupGet(x => x.BottomBase).Returns(80);
            sourceMock.SetupGet(x => x.TopBaseOffset).Returns(10);

            var targetMock = new Mock<ITrapezoidShape>();

            // Act
            sut.Update(targetMock.Object, sourceMock.Object);

            // Assert
            targetMock.VerifySet(x => x.Height = 100, Times.Once);
            targetMock.VerifySet(x => x.TopBase = 40, Times.Once);
            targetMock.VerifySet(x => x.BottomBase = 80, Times.Once);
            targetMock.VerifySet(x => x.TopBaseOffset = 10, Times.Once);
        }
    }
}
