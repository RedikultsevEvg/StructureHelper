using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;

namespace StructureHelperTests.UnitTests.ConvertStrategiesTest.ShapesTests
{
    [TestFixture]
    public class CloneModeTests
    {
        [Test]
        public void Point2D_ShouldSupportAllCloneModesThroughDefaultStrategy()
        {
            var source = new Point2D(Guid.NewGuid()) { X = 1, Y = 2 };
            var strategy = (ICloneStrategy)source;

            var reference = strategy.GetClone(source, CloningMode.Reference);
            var preserved = (IPoint2D)strategy.GetClone(source, CloningMode.PreserveIdentity);
            var newEntity = (IPoint2D)strategy.GetClone(source, CloningMode.NewEntity);

            Assert.Multiple(() =>
            {
                Assert.That(reference, Is.SameAs(source));
                Assert.That(preserved, Is.Not.SameAs(source));
                Assert.That(preserved.Id, Is.EqualTo(source.Id));
                Assert.That(newEntity.Id, Is.Not.EqualTo(source.Id));
            });
        }

        [Test]
        public void ReferenceMode_ShouldReturnSource()
        {
            var source = new RectangleShape(Guid.NewGuid());
            var strategy = new RectangleShapeCloneStrategy();

            var clone = strategy.GetClone(source, CloningMode.Reference);

            Assert.That(clone, Is.SameAs(source));
        }

        [Test]
        public void PreserveIdentityMode_ShouldKeepId()
        {
            var source = new RectangleShape(Guid.NewGuid());
            var strategy = new RectangleShapeCloneStrategy();

            var clone = strategy.GetClone(source, CloningMode.PreserveIdentity);

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(source));
                Assert.That(clone.Id, Is.EqualTo(source.Id));
            });
        }

        [Test]
        public void NewEntityMode_ShouldCreateNewId()
        {
            var source = new RectangleShape(Guid.NewGuid());
            var strategy = new RectangleShapeCloneStrategy();

            var clone = strategy.GetClone(source, CloningMode.NewEntity);

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(source));
                Assert.That(clone.Id, Is.Not.EqualTo(source.Id));
            });
        }
    }
}
