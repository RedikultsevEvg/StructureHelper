using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    using NUnit.Framework;
    using StructureHelperCommon.Infrastructures.Exceptions;
    using StructureHelperCommon.Models.Shapes;
    using System;
    using System.Linq;

    [TestFixture]
    public class LinePolygonRotateStrategyTests
    {
        private LinePolygonShape CreateTriangle()
        {
            var polygon = new LinePolygonShape(Guid.NewGuid());

            polygon.AddVertex(new Vertex(Guid.NewGuid())
            {
                Point = new Point2D(1, 0)
            });

            polygon.AddVertex(new Vertex(Guid.NewGuid())
            {
                Point = new Point2D(0, 1)
            });

            polygon.AddVertex(new Vertex(Guid.NewGuid())
            {
                Point = new Point2D(0, 0)
            });

            polygon.IsClosed = true;

            return polygon;
        }

        [Test]
        public void Convert_NullSource_ThrowsException()
        {
            var strategy = new LinePolygonRotateStrategy(Math.PI / 2);

            Assert.Throws<StructureHelperException>(() =>
                strategy.Convert(null));
        }

        [Test]
        public void Convert_Rotate90DegreesAroundOrigin_WorksCorrectly()
        {
            var polygon = CreateTriangle();

            var strategy = new LinePolygonRotateStrategy(Math.PI / 2);

            var result = strategy.Convert(polygon);

            var vertices = result.Vertices.ToList();

            Assert.That(vertices[0].Point.X, Is.EqualTo(0).Within(1e-10));
            Assert.That(vertices[0].Point.Y, Is.EqualTo(1).Within(1e-10));

            Assert.That(vertices[1].Point.X, Is.EqualTo(-1).Within(1e-10));
            Assert.That(vertices[1].Point.Y, Is.EqualTo(0).Within(1e-10));
        }

        [Test]
        public void Convert_Rotate180Degrees_WorksCorrectly()
        {
            var polygon = CreateTriangle();

            var strategy = new LinePolygonRotateStrategy(Math.PI);

            var result = strategy.Convert(polygon);

            var vertices = result.Vertices.ToList();

            Assert.That(vertices[0].Point.X, Is.EqualTo(-1).Within(1e-10));
            Assert.That(vertices[0].Point.Y, Is.EqualTo(0).Within(1e-10));
        }

        [Test]
        public void Convert_RotateAroundCustomCenter_WorksCorrectly()
        {
            var polygon = new LinePolygonShape(Guid.NewGuid());
            polygon.AddVertex(new Vertex(Guid.NewGuid())
            {
                Point = new Point2D(2, 1)
            });

            polygon.IsClosed = true;

            // Rotate 90° around (1,1)
            var strategy = new LinePolygonRotateStrategy(
                Math.PI / 2,
                centerX: 1,
                centerY: 1);

            var result = strategy.Convert(polygon);

            var v = result.Vertices.First();

            Assert.That(v.Point.X, Is.EqualTo(1).Within(1e-10));
            Assert.That(v.Point.Y, Is.EqualTo(2).Within(1e-10));
        }

        [Test]
        public void Convert_DoesNotModifySourcePolygon()
        {
            var polygon = CreateTriangle();
            var originalX = polygon.Vertices[0].Point.X;
            var originalY = polygon.Vertices[0].Point.Y;

            var strategy = new LinePolygonRotateStrategy(Math.PI / 2);

            strategy.Convert(polygon);

            Assert.That(polygon.Vertices[0].Point.X, Is.EqualTo(originalX));
            Assert.That(polygon.Vertices[0].Point.Y, Is.EqualTo(originalY));
        }

        [Test]
        public void Convert_PreservesIsClosedProperty()
        {
            var polygon = CreateTriangle();
            polygon.IsClosed = true;

            var strategy = new LinePolygonRotateStrategy(Math.PI / 3);

            var result = strategy.Convert(polygon);

            Assert.That(result.IsClosed, Is.True);
        }
    }
}
