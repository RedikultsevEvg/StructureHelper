using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperTests.UnitTests.ForcesTests
{
    [TestFixture]
    public class ThermalActionTemperatureLogicTests
    {
        [Test]
        public void GetTemperature_ShouldComputeLinearFieldRelativeToReferencePoint()
        {
            var action = new ThermalActionStub
            {
                ReferencePoint = new Point2D(1d, 2d),
                RefPointTemperature = 300d,
                GradientX = 4d,
                GradientY = 5d
            };
            var logic = new ThermalIndependentLogic();

            double temperature = logic.GetTemperature(action, new Point2D(2d, 4d));

            Assert.That(temperature, Is.EqualTo(314d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_WhenActionIsNull_ShouldThrowStructureHelperException()
        {
            var logic = new ThermalIndependentLogic();

            Assert.That(() => logic.GetTemperature(null!, new Point2D()), Throws.TypeOf<StructureHelperException>());
        }

        [Test]
        public void GetTemperature_WhenPointIsNull_ShouldThrowStructureHelperException()
        {
            var logic = new ThermalIndependentLogic();
            var action = new ThermalActionStub();

            Assert.That(() => logic.GetTemperature(action, null!), Throws.TypeOf<StructureHelperException>());
        }

        private sealed class ThermalActionStub : IThermalActionTimeIndependend
        {
            public IPoint2D ReferencePoint { get; set; } = new Point2D();

            public double RefPointTemperature { get; set; }

            public double GradientX { get; set; }

            public double GradientY { get; set; }
            public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Guid Id => throw new NotImplementedException();

            public object Clone()
            {
                throw new NotImplementedException();
            }
        }
    }
}
