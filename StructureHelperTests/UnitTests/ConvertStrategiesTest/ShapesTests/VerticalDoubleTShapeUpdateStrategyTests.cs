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
    public class VerticalDoubleTShapeUpdateStrategyTests
    {
        private VerticalDoubleTShapeUpdateStrategy sut;

        [SetUp]
        public void SetUp()
        {
            sut = new VerticalDoubleTShapeUpdateStrategy();
        }

        [Test]
        public void Update_WhenTargetIsNull_Throws()
        {
            var source = new Mock<IVerticalDoubleTShape>().Object;

            Assert.Throws<StructureHelperException>(() =>
                sut.Update(null, source));
            // Replace Exception with StructureHelperException if applicable
        }

        [Test]
        public void Update_WhenSourceIsNull_Throws()
        {
            var target = new Mock<IVerticalDoubleTShape>().Object;

            Assert.Throws<StructureHelperException>(() =>
                sut.Update(target, null));
        }

        [Test]
        public void Update_WhenTargetEqualsSource_DoesNothing()
        {
            var shapeMock = new Mock<IVerticalDoubleTShape>();

            sut.Update(shapeMock.Object, shapeMock.Object);

            shapeMock.VerifySet(x => x.FullHeight = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.WebThickness = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.TopFlangeThickness = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.TopFlangeWidth = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.BottomFlangeThickness = It.IsAny<double>(), Times.Never);
            shapeMock.VerifySet(x => x.BottomFlangeWidth = It.IsAny<double>(), Times.Never);
        }

        [Test]
        public void Update_WhenValidObjects_CopiesAllProperties()
        {
            // Arrange
            var sourceMock = new Mock<IVerticalDoubleTShape>();
            sourceMock.SetupGet(x => x.FullHeight).Returns(500);
            sourceMock.SetupGet(x => x.WebThickness).Returns(20);
            sourceMock.SetupGet(x => x.TopFlangeThickness).Returns(25);
            sourceMock.SetupGet(x => x.TopFlangeWidth).Returns(200);
            sourceMock.SetupGet(x => x.BottomFlangeThickness).Returns(30);
            sourceMock.SetupGet(x => x.BottomFlangeWidth).Returns(180);

            var targetMock = new Mock<IVerticalDoubleTShape>();

            // Act
            sut.Update(targetMock.Object, sourceMock.Object);

            // Assert
            targetMock.VerifySet(x => x.FullHeight = 500, Times.Once);
            targetMock.VerifySet(x => x.WebThickness = 20, Times.Once);
            targetMock.VerifySet(x => x.TopFlangeThickness = 25, Times.Once);
            targetMock.VerifySet(x => x.TopFlangeWidth = 200, Times.Once);
            targetMock.VerifySet(x => x.BottomFlangeThickness = 30, Times.Once);
            targetMock.VerifySet(x => x.BottomFlangeWidth = 180, Times.Once);
        }
    }
}
