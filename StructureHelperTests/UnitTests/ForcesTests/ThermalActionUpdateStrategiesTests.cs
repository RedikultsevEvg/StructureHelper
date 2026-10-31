using NUnit.Framework;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperTests.Infrastructures.Forces;

namespace StructureHelperTests.UnitTests.ForcesTests
{
    [TestFixture]
    public class ThermalActionUpdateStrategiesTests
    {
        [Test]
        public void IndependentStrategy_ShouldCopyScalarsAndReferencePoint()
        {
            ThermalActionTimeIndependent target = ThermalActionTestData.CreateAction(
                refPointTemperature: 280d, gradientX: 1d, gradientY: 2d, referenceX: 0d, referenceY: 0d);
            ThermalActionTimeIndependent source = ThermalActionTestData.CreateAction(
                refPointTemperature: 300d, gradientX: 3d, gradientY: 4d, referenceX: 5d, referenceY: 6d);
            var strategy = new ThermalActionTimeIndependentUpdateStrategy();

            strategy.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.RefPointTemperature, Is.EqualTo(300d));
                Assert.That(target.GradientX, Is.EqualTo(3d));
                Assert.That(target.GradientY, Is.EqualTo(4d));
                Assert.That(target.ReferencePoint.X, Is.EqualTo(5d));
                Assert.That(target.ReferencePoint.Y, Is.EqualTo(6d));
                Assert.That(target.ReferencePoint, Is.Not.SameAs(source.ReferencePoint));
            });
        }

        [Test]
        public void IndependentStrategy_WhenUpdateChildrenIsFalse_ShouldKeepReferencePoint()
        {
            ThermalActionTimeIndependent target = ThermalActionTestData.CreateAction(
                refPointTemperature: 280d, referenceX: 1d, referenceY: 2d);
            ThermalActionTimeIndependent source = ThermalActionTestData.CreateAction(
                refPointTemperature: 300d, referenceX: 5d, referenceY: 6d);
            var strategy = new ThermalActionTimeIndependentUpdateStrategy { UpdateChildren = false };

            strategy.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.RefPointTemperature, Is.EqualTo(300d));
                Assert.That(target.ReferencePoint.X, Is.EqualTo(1d));
                Assert.That(target.ReferencePoint.Y, Is.EqualTo(2d));
            });
        }

        [Test]
        public void PairStrategy_ShouldCopyTimeAndUpdateThermalAction()
        {
            var targetAction = ThermalActionTestData.CreateAction(280d);
            var sourceAction = ThermalActionTestData.CreateAction(300d, gradientX: 3d, gradientY: 4d, referenceX: 5d, referenceY: 6d);
            var target = new ThermalActionTimePair(0d, targetAction);
            var source = new ThermalActionTimePair(10d, sourceAction);
            var strategy = new ThermalActionTimePairUpdateStrategy();

            strategy.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Time, Is.EqualTo(10d));
                Assert.That(targetAction.RefPointTemperature, Is.EqualTo(300d));
                Assert.That(targetAction.GradientX, Is.EqualTo(3d));
                Assert.That(targetAction.ReferencePoint.X, Is.EqualTo(5d));
            });
        }

        [Test]
        public void PairStrategy_WhenUpdateChildrenIsFalse_ShouldKeepThermalAction()
        {
            var targetAction = ThermalActionTestData.CreateAction(280d);
            var sourceAction = ThermalActionTestData.CreateAction(300d);
            var target = new ThermalActionTimePair(0d, targetAction);
            var source = new ThermalActionTimePair(10d, sourceAction);
            var strategy = new ThermalActionTimePairUpdateStrategy { UpdateChildren = false };

            strategy.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Time, Is.EqualTo(10d));
                Assert.That(targetAction.RefPointTemperature, Is.EqualTo(280d));
            });
        }

        [Test]
        public void ListStrategy_ShouldReplacePairsWithClones()
        {
            var target = new ThermalActionTimeList { Name = "target" };
            target.TimePairs.Add(ThermalActionTestData.CreatePair(0d, 280d));
            var source = new ThermalActionTimeList { Name = "source" };
            source.TimePairs.Add(ThermalActionTestData.CreatePair(0d, 290d));
            source.TimePairs.Add(ThermalActionTestData.CreatePair(10d, 300d));
            var strategy = new ThermalActionTimeListUpdateStrategy();

            strategy.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Name, Is.EqualTo("source"));
                Assert.That(target.TimePairs, Has.Count.EqualTo(2));
                Assert.That(target.TimePairs[0].Time, Is.EqualTo(0d));
                Assert.That(target.TimePairs[1].Time, Is.EqualTo(10d));
                Assert.That(target.TimePairs[0].ThermalAction, Is.Not.SameAs(source.TimePairs[0].ThermalAction));
                Assert.That(target.TimePairs[1].ThermalAction.RefPointTemperature, Is.EqualTo(300d));
            });
        }

        [Test]
        public void ListStrategy_WhenUpdateChildrenIsFalse_ShouldKeepPairs()
        {
            var target = new ThermalActionTimeList { Name = "target" };
            target.TimePairs.Add(ThermalActionTestData.CreatePair(0d, 280d));
            var source = new ThermalActionTimeList { Name = "source" };
            source.TimePairs.Add(ThermalActionTestData.CreatePair(10d, 300d));
            var strategy = new ThermalActionTimeListUpdateStrategy { UpdateChildren = false };

            strategy.Update(target, source);

            Assert.Multiple(() =>
            {
                Assert.That(target.Name, Is.EqualTo("source"));
                Assert.That(target.TimePairs, Has.Count.EqualTo(1));
                Assert.That(target.TimePairs[0].Time, Is.EqualTo(0d));
                Assert.That(target.TimePairs[0].ThermalAction.RefPointTemperature, Is.EqualTo(280d));
            });
        }
    }
}
