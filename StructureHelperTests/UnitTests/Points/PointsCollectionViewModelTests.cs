using NUnit.Framework;
using StructureHelper.Windows.Points;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperTests.UnitTests.Points
{
    [TestFixture]
    public class PointsCollectionViewModelTests
    {
        [Test]
        public void AddPoint_ShouldAppendPointAndSelectIt()
        {
            var viewModel = new PointsCollectionViewModel();

            viewModel.AddPoint();
            viewModel.AddPoint();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Points, Has.Count.EqualTo(2));
                Assert.That(viewModel.PointsCount, Is.EqualTo(2));
                Assert.That(viewModel.SelectedPoint, Is.SameAs(viewModel.Points[1]));
            });
        }

        [Test]
        public void DeletePoint_ShouldRemoveSelectedPointAndSelectNeighbour()
        {
            var viewModel = new PointsCollectionViewModel();
            viewModel.AddPoint();
            viewModel.AddPoint();
            viewModel.AddPoint();
            Point2D removedPoint = viewModel.Points[1];
            viewModel.SelectedPoint = removedPoint;

            viewModel.DeletePoint();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Points, Has.Count.EqualTo(2));
                Assert.That(viewModel.Points, Does.Not.Contain(removedPoint));
                Assert.That(viewModel.SelectedPoint, Is.SameAs(viewModel.Points[1]));
            });
        }

        [Test]
        public void DeletePoint_WhenLastPointIsDeleted_ShouldClearSelection()
        {
            var viewModel = new PointsCollectionViewModel();
            viewModel.AddPoint();
            viewModel.SelectedPoint = viewModel.Points[0];

            viewModel.DeletePoint();

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.Points, Is.Empty);
                Assert.That(viewModel.SelectedPoint, Is.Null);
            });
        }
    }
}
