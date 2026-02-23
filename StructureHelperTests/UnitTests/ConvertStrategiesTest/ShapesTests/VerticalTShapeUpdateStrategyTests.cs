using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    using Moq;
    using NUnit.Framework;
    using StructureHelperCommon.Infrastructures.Exceptions;
    using StructureHelperCommon.Models.Shapes;

        [TestFixture]
        public class VerticalTShapeUpdateStrategyTests
        {
            private VerticalTShapeUpdateStrategy sut;

            [SetUp]
            public void SetUp()
            {
                sut = new VerticalTShapeUpdateStrategy();
            }

            [Test]
            public void Update_WhenSourceIsNull_Throws()
            {
                var target = new Mock<IVerticalTShape>().Object;

                Assert.Throws<StructureHelperException>(() => sut.Update(target, null));
                // Replace Exception with StructureHelperException if applicable
            }

            [Test]
            public void Update_WhenTargetIsNull_Throws()
            {
                var source = new Mock<IVerticalTShape>().Object;

                Assert.Throws<StructureHelperException>(() => sut.Update(null, source));
            }

            [Test]
            public void Update_WhenTargetEqualsSource_DoesNothing()
            {
                var shapeMock = new Mock<IVerticalTShape>();

                sut.Update(shapeMock.Object, shapeMock.Object);

                // No property set should occur
                shapeMock.VerifySet(x => x.FullHeight = It.IsAny<double>(), Times.Never);
                shapeMock.VerifySet(x => x.WebWidth = It.IsAny<double>(), Times.Never);
                shapeMock.VerifySet(x => x.FlangeHeight = It.IsAny<double>(), Times.Never);
                shapeMock.VerifySet(x => x.FlangeWidth = It.IsAny<double>(), Times.Never);
                shapeMock.VerifySet(x => x.FlangeXOffset = It.IsAny<double>(), Times.Never);
            }

            [Test]
            public void Update_WhenValidObjects_CopiesAllProperties()
            {
                // Arrange
                var sourceMock = new Mock<IVerticalTShape>();
                sourceMock.SetupGet(x => x.FullHeight).Returns(100);
                sourceMock.SetupGet(x => x.WebWidth).Returns(20);
                sourceMock.SetupGet(x => x.FlangeHeight).Returns(30);
                sourceMock.SetupGet(x => x.FlangeWidth).Returns(50);
                sourceMock.SetupGet(x => x.FlangeXOffset).Returns(10);

                var targetMock = new Mock<IVerticalTShape>();

                // Act
                sut.Update(targetMock.Object, sourceMock.Object);

                // Assert
                targetMock.VerifySet(x => x.FullHeight = 100, Times.Once);
                targetMock.VerifySet(x => x.WebWidth = 20, Times.Once);
                targetMock.VerifySet(x => x.FlangeHeight = 30, Times.Once);
                targetMock.VerifySet(x => x.FlangeWidth = 50, Times.Once);
                targetMock.VerifySet(x => x.FlangeXOffset = 10, Times.Once);
            }
        }
}
