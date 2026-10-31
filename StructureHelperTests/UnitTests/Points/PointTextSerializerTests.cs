using NUnit.Framework;
using StructureHelper.Services.Points;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.Points
{
    [TestFixture]
    public class PointTextSerializerTests
    {
        [Test]
        public void Serialize_ShouldWriteHeaderAndMillimetres()
        {
            string text = PointTextSerializer.Serialize(new[] { new Point2D(0.001d, 0.002d) });
            string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Assert.Multiple(() =>
            {
                Assert.That(lines, Has.Length.EqualTo(2));
                Assert.That(lines[0], Is.EqualTo(PointTextSerializer.Header));
                Assert.That(lines[1], Is.EqualTo("1\t2"));
            });
        }

        [Test]
        public void Deserialize_ShouldParseTabSeparatedTextAndSkipHeader()
        {
            const string text = "X, mm\tY, mm\r\n1\t2\r\n-3500\t4250\r\n";

            List<Point2D> points = PointTextSerializer.Deserialize(text);

            Assert.Multiple(() =>
            {
                Assert.That(points, Has.Count.EqualTo(2));
                Assert.That(points[0].X, Is.EqualTo(0.001d).Within(1e-12));
                Assert.That(points[0].Y, Is.EqualTo(0.002d).Within(1e-12));
                Assert.That(points[1].X, Is.EqualTo(-3.5d).Within(1e-12));
                Assert.That(points[1].Y, Is.EqualTo(4.25d).Within(1e-12));
            });
        }

        [Test]
        public void Deserialize_ShouldAcceptSemicolonSeparator()
        {
            List<Point2D> points = PointTextSerializer.Deserialize("10;20");

            Assert.Multiple(() =>
            {
                Assert.That(points, Has.Count.EqualTo(1));
                Assert.That(points[0].X, Is.EqualTo(0.01d).Within(1e-12));
                Assert.That(points[0].Y, Is.EqualTo(0.02d).Within(1e-12));
            });
        }

        [Test]
        public void Deserialize_WhenTextContainsNoValidPoints_ShouldReturnEmptyCollection()
        {
            Assert.Multiple(() =>
            {
                Assert.That(PointTextSerializer.Deserialize("abc\ndef"), Is.Empty);
                Assert.That(PointTextSerializer.Deserialize(""), Is.Empty);
                Assert.That(PointTextSerializer.Deserialize(null), Is.Empty);
            });
        }
    }
}
