using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperTests.Infrastructures.Forces;
using System;

namespace StructureHelperTests.UnitTests.ForcesTests
{
    [TestFixture]
    public class ThermalActionTimeIndependentTests
    {
        [Test]
        public void GetTemperature_ShouldComputeTemperatureInKelvins()
        {
            ThermalActionTimeIndependent action = ThermalActionTestData.CreateAction(
                refPointTemperature: 293.15d,
                gradientX: 2d,
                gradientY: 3d,
                referenceX: 1d,
                referenceY: 1d);

            var logic = new ThermalIndependentLogic();
            double temperature = logic.GetTemperature(action, new Point2D(3d, 4d));

            Assert.That(temperature, Is.EqualTo(306.15d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_ShouldReturnReferenceTemperatureAtReferencePoint()
        {
            ThermalActionTimeIndependent action = ThermalActionTestData.CreateAction(
                refPointTemperature: 300d,
                gradientX: 5d,
                gradientY: -2d,
                referenceX: 2d,
                referenceY: 3d);

            var logic = new ThermalIndependentLogic();
            double temperature = logic.GetTemperature(action, new Point2D(2d, 3d));

            Assert.That(temperature, Is.EqualTo(300d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_ShouldNotDependOnTime()
        {
            ThermalActionTimeIndependent action = ThermalActionTestData.CreateAction(
                refPointTemperature: 293.15d,
                gradientX: 2d,
                gradientY: 3d);
            var point = new Point2D(1d, 2d);

            var logic = new GetTemperatureByThermalIndependentTimeLogic();
            double atZeroTime = logic.GetTemperature(action, point, 0d);
            double atLaterTime = logic.GetTemperature(action, point, 3600d);

            Assert.That(atLaterTime, Is.EqualTo(atZeroTime).Within(1e-12));
        }

        [Test]
        public void Clone_ShouldCopyPropertiesAndCreateIndependentInstance()
        {
            ThermalActionTimeIndependent action = ThermalActionTestData.CreateAction(
                refPointTemperature: 300d,
                gradientX: 2d,
                gradientY: 3d,
                referenceX: 1d,
                referenceY: 2d);
            action.Name = "Thermal action";

            var clone = (ThermalActionTimeIndependent)action.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(action));
                Assert.That(clone.Id, Is.Not.EqualTo(action.Id));
                Assert.That(clone.Name, Is.EqualTo(action.Name));
                Assert.That(clone.RefPointTemperature, Is.EqualTo(action.RefPointTemperature));
                Assert.That(clone.GradientX, Is.EqualTo(action.GradientX));
                Assert.That(clone.GradientY, Is.EqualTo(action.GradientY));
                Assert.That(clone.ReferencePoint, Is.Not.SameAs(action.ReferencePoint));
                Assert.That(clone.ReferencePoint.X, Is.EqualTo(action.ReferencePoint.X));
                Assert.That(clone.ReferencePoint.Y, Is.EqualTo(action.ReferencePoint.Y));
            });
        }

        [Test]
        public void GetTemperature_WhenPointIsNull_ShouldThrowStructureHelperException()
        {
            ThermalActionTimeIndependent action = ThermalActionTestData.CreateAction(293.15d);
            var logic = new GetTemperatureByThermalIndependentTimeLogic();
            Assert.That(() => logic.GetTemperature(action, null!, 0d), Throws.TypeOf<StructureHelperException>());
        }
    }
}
