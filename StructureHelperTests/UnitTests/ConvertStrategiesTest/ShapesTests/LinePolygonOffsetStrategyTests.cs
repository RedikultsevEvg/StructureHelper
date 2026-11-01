using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    [TestFixture]
    public class LinePolygonOffsetStrategyTests
    {
        [Test]
        public void Convert_NullSource_ThrowsException()
        {
            var strategy = new LinePolygonOffsetStrategy(0.1d);

            Assert.Throws<StructureHelperException>(() => strategy.Convert(null!));
        }

        [Test]
        public void Convert_ZeroDistance_ReturnsCopy()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (1d, 0d), (1d, 1d), (0d, 1d));

            ILinePolygonShape result = new LinePolygonOffsetStrategy(0d).Convert(source);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.SameAs(source));
                Assert.That(result.IsClosed, Is.True);
                AssertPoints(result, (0d, 0d), (1d, 0d), (1d, 1d), (0d, 1d));
            });
        }

        [Test]
        public void Convert_CounterClockwiseSquare_OffsetsOutward()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (1d, 0d), (1d, 1d), (0d, 1d));

            ILinePolygonShape result = new LinePolygonOffsetStrategy(1d).Convert(source);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsClosed, Is.True);
                AssertPoints(result, (-1d, -1d), (2d, -1d), (2d, 2d), (-1d, 2d));
            });
        }

        [Test]
        public void Convert_ClockwiseSquare_OffsetsOutward()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (0d, 1d), (1d, 1d), (1d, 0d));

            ILinePolygonShape result = new LinePolygonOffsetStrategy(1d).Convert(source);

            AssertPoints(result, (-1d, -1d), (-1d, 2d), (2d, 2d), (2d, -1d));
        }

        [Test]
        public void Convert_NegativeDistance_OffsetsInward()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (1d, 0d), (1d, 1d), (0d, 1d));

            ILinePolygonShape result = new LinePolygonOffsetStrategy(-0.25d).Convert(source);

            AssertPoints(result, (0.25d, 0.25d), (0.75d, 0.25d), (0.75d, 0.75d), (0.25d, 0.75d));
        }

        [Test]
        public void Convert_DoesNotModifySourcePolygon()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (1d, 0d), (1d, 1d), (0d, 1d));

            _ = new LinePolygonOffsetStrategy(0.5d).Convert(source);

            AssertPoints(source, (0d, 0d), (1d, 0d), (1d, 1d), (0d, 1d));
        }

        [Test]
        public void Convert_OpenPolygon_ThrowsException()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (1d, 0d), (1d, 1d));
            source.IsClosed = false;

            Assert.Throws<StructureHelperException>(
                () => new LinePolygonOffsetStrategy(0.1d).Convert(source));
        }

        [Test]
        public void Convert_DegeneratePolygon_ThrowsException()
        {
            ILinePolygonShape source = CreatePolygon(
                (0d, 0d), (1d, 0d), (2d, 0d));

            Assert.Throws<StructureHelperException>(
                () => new LinePolygonOffsetStrategy(0.1d).Convert(source));
        }

        private static ILinePolygonShape CreatePolygon(params (double x, double y)[] points)
        {
            var polygon = new LinePolygonShape(Guid.NewGuid());
            foreach ((double x, double y) in points)
            {
                polygon.AddVertex(new Vertex(x, y));
            }
            return polygon;
        }

        private static void AssertPoints(ILinePolygonShape polygon, params (double x, double y)[] expected)
        {
            Assert.That(polygon.Vertices, Has.Count.EqualTo(expected.Length));
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.That(polygon.Vertices[i].Point.X, Is.EqualTo(expected[i].x).Within(1e-9), $"X at vertex {i}");
                Assert.That(polygon.Vertices[i].Point.Y, Is.EqualTo(expected[i].y).Within(1e-9), $"Y at vertex {i}");
            }
        }
    }
}
