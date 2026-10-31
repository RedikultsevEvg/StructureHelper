using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperTests.Infrastructures.Forces;
using System;

namespace StructureHelperTests.UnitTests.ForcesTests
{
    [TestFixture]
    public class ThermalActionTimeInterpolationLogicTests
    {
        private readonly IThermalPairTimeInterpolationLogic logic = new ThermalPairTimeInterpolationLogic();

        [Test]
        public void GetTemperature_ShouldInterpolateBetweenTimePairs()
        {
            var pairs = new[]
            {
                ThermalActionTestData.CreatePair(0d, 293.15d),
                ThermalActionTestData.CreatePair(10d, 303.15d)
            };

            double temperature = logic.GetTemperature(pairs, new Point2D(), 5d);

            Assert.That(temperature, Is.EqualTo(298.15d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_AtDefinedTime_ShouldReturnExactTemperature()
        {
            var pairs = new[]
            {
                ThermalActionTestData.CreatePair(0d, 293.15d),
                ThermalActionTestData.CreatePair(10d, 303.15d)
            };

            Assert.Multiple(() =>
            {
                Assert.That(logic.GetTemperature(pairs, new Point2D(), 0d), Is.EqualTo(293.15d).Within(1e-12));
                Assert.That(logic.GetTemperature(pairs, new Point2D(), 10d), Is.EqualTo(303.15d).Within(1e-12));
            });
        }

        [Test]
        public void GetTemperature_ShouldSortPairsByTime()
        {
            var pairs = new[]
            {
                ThermalActionTestData.CreatePair(10d, 303.15d),
                ThermalActionTestData.CreatePair(0d, 293.15d)
            };

            double temperature = logic.GetTemperature(pairs, new Point2D(), 5d);

            Assert.That(temperature, Is.EqualTo(298.15d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_ShouldInterpolateTemperatureFieldAtPoint()
        {
            var pairs = new[]
            {
                new ThermalActionTimePair(0d, ThermalActionTestData.CreateAction(300d, gradientX: 0d)),
                new ThermalActionTimePair(10d, ThermalActionTestData.CreateAction(320d, gradientX: 2d))
            };

            double temperature = logic.GetTemperature(pairs, new Point2D(1d, 0d), 5d);

            Assert.That(temperature, Is.EqualTo(311d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_WhenTimeIsBelowRange_ShouldThrowStructureHelperException()
        {
            var pairs = new[]
            {
                ThermalActionTestData.CreatePair(0d, 293.15d),
                ThermalActionTestData.CreatePair(10d, 303.15d)
            };

            Assert.That(() => logic.GetTemperature(pairs, new Point2D(), -0.001d),
                Throws.TypeOf<StructureHelperException>());
        }

        [Test]
        public void GetTemperature_WhenTimeIsAboveRange_ShouldThrowStructureHelperException()
        {
            var pairs = new[]
            {
                ThermalActionTestData.CreatePair(0d, 293.15d),
                ThermalActionTestData.CreatePair(10d, 303.15d)
            };

            Assert.That(() => logic.GetTemperature(pairs, new Point2D(), 10.001d),
                Throws.TypeOf<StructureHelperException>());
        }

        [Test]
        public void GetTemperature_WhenPairsAreEmpty_ShouldThrowStructureHelperException()
        {
            Assert.That(() => logic.GetTemperature(Array.Empty<IThermalActionTimePair>(), new Point2D(), 0d),
                Throws.TypeOf<StructureHelperException>());
        }

        [Test]
        public void GetTemperature_WhenTimeIsNaN_ShouldThrowStructureHelperException()
        {
            var pairs = new[]
            {
                ThermalActionTestData.CreatePair(0d, 293.15d),
                ThermalActionTestData.CreatePair(10d, 303.15d)
            };

            Assert.That(() => logic.GetTemperature(pairs, new Point2D(), double.NaN),
                Throws.TypeOf<StructureHelperException>());
        }

        [Test]
        public void ThermalActionTimePair_WhenActionIsNull_ShouldThrowStructureHelperException()
        {
            Assert.That(() => new ThermalActionTimePair(0d, null!), Throws.TypeOf<StructureHelperException>());
        }
    }
}
