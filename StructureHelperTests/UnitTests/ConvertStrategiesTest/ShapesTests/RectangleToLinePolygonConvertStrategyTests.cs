using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    using NUnit.Framework;
    using StructureHelperCommon.Infrastructures.Exceptions;
    using StructureHelperCommon.Models.Shapes;

    [TestFixture]
    public class RectangleToLinePolygonConvertStrategyTests
    {
        private RectangleToLinePolygonConvertStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _strategy = new RectangleToLinePolygonConvertStrategy();
        }

        [Test]
        public void Convert_NullSource_ThrowsStructureHelperException()
        {
            Assert.Throws<StructureHelperException>(() =>
                _strategy.Convert(null));
        }

        [Test]
        public void Convert_ValidRectangle_CreatesPolygonWithFourVertices()
        {
            var rectangle = new RectangleShape
            {
                Width = 10,
                Height = 4
            };

            var result = _strategy.Convert(rectangle);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Vertices.Count, Is.EqualTo(4));
        }

        [Test]
        public void Convert_ValidRectangle_SetsPolygonClosed()
        {
            var rectangle = new RectangleShape
            {
                Width = 10,
                Height = 4
            };

            var result = _strategy.Convert(rectangle);

            Assert.That(result.IsClosed, Is.True);
        }

        [Test]
        public void Convert_Rectangle_CreatesCorrectVertexCoordinates()
        {
            var rectangle = new RectangleShape
            {
                Width = 10,
                Height = 4
            };

            var result = _strategy.Convert(rectangle);

            // halfWidth = 5
            // halfHeight = 2

            Assert.That(result.Vertices[0].Point.X, Is.EqualTo(-5));
            Assert.That(result.Vertices[0].Point.Y, Is.EqualTo(-2));

            Assert.That(result.Vertices[1].Point.X, Is.EqualTo(5));
            Assert.That(result.Vertices[1].Point.Y, Is.EqualTo(-2));

            Assert.That(result.Vertices[2].Point.X, Is.EqualTo(5));
            Assert.That(result.Vertices[2].Point.Y, Is.EqualTo(2));

            Assert.That(result.Vertices[3].Point.X, Is.EqualTo(-5));
            Assert.That(result.Vertices[3].Point.Y, Is.EqualTo(2));
        }

        [Test]
        public void Convert_NonSquareRectangle_CalculatesCorrectHalfSizes()
        {
            var rectangle = new RectangleShape
            {
                Width = 8,
                Height = 6
            };

            var result = _strategy.Convert(rectangle);

            // halfWidth = 4
            // halfHeight = 3

            Assert.That(result.Vertices[0].Point.X, Is.EqualTo(-4));
            Assert.That(result.Vertices[0].Point.Y, Is.EqualTo(-3));

            Assert.That(result.Vertices[2].Point.X, Is.EqualTo(4));
            Assert.That(result.Vertices[2].Point.Y, Is.EqualTo(3));
        }
    }
}
