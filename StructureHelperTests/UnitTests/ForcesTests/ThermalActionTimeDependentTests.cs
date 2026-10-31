using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperTests.Infrastructures.Forces;

namespace StructureHelperTests.UnitTests.ForcesTests
{
    [TestFixture]
    public class ThermalActionTimeDependentTests
    {
        [Test]
        public void GetTemperature_ShouldUseTimePairsAndInterpolation()
        {
            var action = new ThermalActionTimeList { Name = "Heating" };
            action.TimePairs.Add(ThermalActionTestData.CreatePair(0d, 293.15d));
            action.TimePairs.Add(ThermalActionTestData.CreatePair(10d, 303.15d));

            var logic = new ThermalPairTimeInterpolationLogic();
            double temperature = logic.GetTemperature(action.TimePairs, new Point2D(), 5d);

            Assert.That(temperature, Is.EqualTo(298.15d).Within(1e-12));
        }

        [Test]
        public void GetTemperature_WhenTimeIsOutOfRange_ShouldThrowStructureHelperException()
        {
            var action = new ThermalActionTimeList();
            action.TimePairs.Add(ThermalActionTestData.CreatePair(0d, 293.15d));
            action.TimePairs.Add(ThermalActionTestData.CreatePair(10d, 303.15d));

            var logic = new ThermalPairTimeInterpolationLogic();
            Assert.That(() => logic.GetTemperature(action.TimePairs, new Point2D(), 11d),
                Throws.TypeOf<StructureHelperException>());
        }

        [Test]
        public void Clone_ShouldCopyPairsAndCloneActions()
        {
            var action = new ThermalActionTimeList { Name = "Heating" };
            action.TimePairs.Add(ThermalActionTestData.CreatePair(0d, 293.15d));
            action.TimePairs.Add(ThermalActionTestData.CreatePair(10d, 303.15d));

            var clone = (ThermalActionTimeList)action.Clone();

            Assert.Multiple(() =>
            {
                Assert.That(clone, Is.Not.SameAs(action));
                Assert.That(clone.Name, Is.EqualTo(action.Name));
                Assert.That(clone.TimePairs, Has.Count.EqualTo(2));
                Assert.That(clone.TimePairs[0].Time, Is.EqualTo(0d));
                Assert.That(clone.TimePairs[1].Time, Is.EqualTo(10d));
                Assert.That(clone.TimePairs[0].ThermalAction, Is.Not.SameAs(action.TimePairs[0].ThermalAction));
                Assert.That(clone.TimePairs[1].ThermalAction.RefPointTemperature, Is.EqualTo(303.15d));
            });
        }
    }
}
