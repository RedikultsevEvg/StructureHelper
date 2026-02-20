using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    using NUnit.Framework;
    using StructureHelperCommon.Infrastructures.Exceptions;
    using StructureHelperCommon.Models.Shapes;

    [TestFixture]
    public class TrapezoidToLinePolygonConvertStrategyTests
    {
        private TrapezoidShapeToPolygonConvertStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _strategy = new TrapezoidShapeToPolygonConvertStrategy();
        }

        #region Null tests

        [Test]
        public void Convert_NullSource_ThrowsStructureHelperException()
        {
            Assert.Throws<StructureHelperException>(() =>
                _strategy.Convert(null));
        }

        #endregion

        #region Validation tests

        [Test]
        public void Convert_NegativeBottomBase_ThrowsException()
        {
            var trapezoid = CreateValidTrapezoid();
            trapezoid.BottomBase = -1;

            Assert.Throws<StructureHelperException>(() =>
                _strategy.Convert(trapezoid));
        }

        [Test]
        public void Convert_NegativeTopBase_ThrowsException()
        {
            var trapezoid = CreateValidTrapezoid();
            trapezoid.TopBase = -1;

            Assert.Throws<StructureHelperException>(() =>
                _strategy.Convert(trapezoid));
        }

        [Test]
        public void Convert_NegativeHeight_ThrowsException()
        {
            var trapezoid = CreateValidTrapezoid();
            trapezoid.Height = -1;

            Assert.Throws<StructureHelperException>(() =>
                _strategy.Convert(trapezoid));
        }

        #endregion

        #region Geometry tests

        [Test]
        public void Convert_ValidTrapezoid_CreatesFourVertices()
        {
            var trapezoid = CreateValidTrapezoid();

            var result = _strategy.Convert(trapezoid);

            Assert.That(result.Vertices.Count, Is.EqualTo(4));
        }

        [Test]
        public void Convert_ValidTrapezoid_SetsClosedTrue()
        {
            var trapezoid = CreateValidTrapezoid();

            var result = _strategy.Convert(trapezoid);

            Assert.That(result.IsClosed, Is.True);
        }

        [Test]
        public void Convert_IsoscelesTrapezoid_CreatesCorrectCoordinates()
        {
            var trapezoid = new TrapezoidShape
            {
                BottomBase = 10,
                TopBase = 6,
                Height = 4,
                TopBaseOffset = 0
            };

            var result = _strategy.Convert(trapezoid);

            // halfBottom = 5
            // halfTop = 3

            Assert.That(result.Vertices[0].Point.X, Is.EqualTo(-5));
            Assert.That(result.Vertices[0].Point.Y, Is.EqualTo(0));

            Assert.That(result.Vertices[1].Point.X, Is.EqualTo(5));
            Assert.That(result.Vertices[1].Point.Y, Is.EqualTo(0));

            Assert.That(result.Vertices[2].Point.X, Is.EqualTo(3));
            Assert.That(result.Vertices[2].Point.Y, Is.EqualTo(4));

            Assert.That(result.Vertices[3].Point.X, Is.EqualTo(-3));
            Assert.That(result.Vertices[3].Point.Y, Is.EqualTo(4));
        }

        [Test]
        public void Convert_AsymmetricTrapezoid_RespectsOffset()
        {
            var trapezoid = new TrapezoidShape
            {
                BottomBase = 10,
                TopBase = 6,
                Height = 4,
                TopBaseOffset = 2
            };

            var result = _strategy.Convert(trapezoid);

            // topCenterX = 2
            // halfTop = 3

            Assert.That(result.Vertices[2].Point.X, Is.EqualTo(5));  // 2 + 3
            Assert.That(result.Vertices[3].Point.X, Is.EqualTo(-1)); // 2 - 3
        }

        #endregion

        #region Helpers

        private TrapezoidShape CreateValidTrapezoid()
        {
            return new TrapezoidShape
            {
                BottomBase = 10,
                TopBase = 6,
                Height = 4,
                TopBaseOffset = 0
            };
        }

        #endregion
    }
}
