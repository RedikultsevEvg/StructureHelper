using NUnit.Framework;
using StructureHelper.Windows.ViewModels;
using System.Collections.Generic;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class SelectItemVMOrderTests
    {
        private List<string> collection = null!;
        private SelectItemVM<string> viewModel = null!;

        [SetUp]
        public void SetUp()
        {
            collection = ["A", "B", "C", "D"];
            viewModel = new SelectItemVM<string>(collection);
        }

        [Test]
        public void MoveUp_ShouldMoveSelectedItemAndKeepBothCollectionsInSync()
        {
            viewModel.SelectedItem = "B";

            bool moved = viewModel.MoveUp();

            Assert.Multiple(() =>
            {
                Assert.That(moved, Is.True);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "B", "A", "C", "D" }));
                Assert.That(collection, Is.EqualTo(new[] { "B", "A", "C", "D" }));
                Assert.That(viewModel.SelectedItem, Is.EqualTo("B"));
            });
        }

        [Test]
        public void MoveDown_ShouldMoveSelectedItemAndKeepBothCollectionsInSync()
        {
            viewModel.SelectedItem = "B";

            bool moved = viewModel.MoveDown();

            Assert.Multiple(() =>
            {
                Assert.That(moved, Is.True);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "C", "B", "D" }));
                Assert.That(collection, Is.EqualTo(new[] { "A", "C", "B", "D" }));
                Assert.That(viewModel.SelectedItem, Is.EqualTo("B"));
            });
        }

        [Test]
        public void MoveToFirst_ShouldMoveSelectedItemToTheBeginning()
        {
            viewModel.SelectedItem = "C";

            bool moved = viewModel.MoveToFirst();

            Assert.Multiple(() =>
            {
                Assert.That(moved, Is.True);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "C", "A", "B", "D" }));
                Assert.That(collection, Is.EqualTo(new[] { "C", "A", "B", "D" }));
                Assert.That(viewModel.SelectedItem, Is.EqualTo("C"));
            });
        }

        [Test]
        public void MoveToEnd_ShouldMoveSelectedItemToTheEnd()
        {
            viewModel.SelectedItem = "B";

            bool moved = viewModel.MoveToEnd();

            Assert.Multiple(() =>
            {
                Assert.That(moved, Is.True);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "C", "D", "B" }));
                Assert.That(collection, Is.EqualTo(new[] { "A", "C", "D", "B" }));
                Assert.That(viewModel.SelectedItem, Is.EqualTo("B"));
            });
        }

        [Test]
        public void MoveUp_WhenSelectedItemIsFirst_ShouldNotChangeOrder()
        {
            viewModel.SelectedItem = "A";

            bool moved = viewModel.MoveUp();

            Assert.Multiple(() =>
            {
                Assert.That(moved, Is.False);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "B", "C", "D" }));
                Assert.That(collection, Is.EqualTo(new[] { "A", "B", "C", "D" }));
            });
        }

        [Test]
        public void MoveDown_WhenSelectedItemIsLast_ShouldNotChangeOrder()
        {
            viewModel.SelectedItem = "D";

            bool moved = viewModel.MoveDown();

            Assert.Multiple(() =>
            {
                Assert.That(moved, Is.False);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "B", "C", "D" }));
                Assert.That(collection, Is.EqualTo(new[] { "A", "B", "C", "D" }));
            });
        }

        [Test]
        public void MoveToBoundary_WhenItemIsAlreadyAtBoundary_ShouldNotChangeOrder()
        {
            viewModel.SelectedItem = "A";
            bool movedToFirst = viewModel.MoveToFirst();

            viewModel.SelectedItem = "D";
            bool movedToEnd = viewModel.MoveToEnd();

            Assert.Multiple(() =>
            {
                Assert.That(movedToFirst, Is.False);
                Assert.That(movedToEnd, Is.False);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "B", "C", "D" }));
            });
        }

        [Test]
        public void MoveMethods_WhenNothingIsSelected_ShouldReturnFalse()
        {
            viewModel.SelectedItem = null!;

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.MoveUp(), Is.False);
                Assert.That(viewModel.MoveDown(), Is.False);
                Assert.That(viewModel.MoveToFirst(), Is.False);
                Assert.That(viewModel.MoveToEnd(), Is.False);
                Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "B", "C", "D" }));
            });
        }

        [Test]
        public void MoveMethods_WhenCollectionHasSingleItem_ShouldReturnFalse()
        {
            var singleCollection = new List<string> { "A" };
            var singleViewModel = new SelectItemVM<string>(singleCollection);
            singleViewModel.SelectedItem = "A";

            Assert.Multiple(() =>
            {
                Assert.That(singleViewModel.MoveUp(), Is.False);
                Assert.That(singleViewModel.MoveDown(), Is.False);
                Assert.That(singleViewModel.MoveToFirst(), Is.False);
                Assert.That(singleViewModel.MoveToEnd(), Is.False);
            });
        }

        [Test]
        public void MoveCommands_ShouldReflectSelectedItemPosition()
        {
            viewModel.SelectedItem = "A";
            Assert.Multiple(() =>
            {
                Assert.That(viewModel.MoveUpCommand.CanExecute(null), Is.False);
                Assert.That(viewModel.MoveToFirstCommand.CanExecute(null), Is.False);
                Assert.That(viewModel.MoveDownCommand.CanExecute(null), Is.True);
                Assert.That(viewModel.MoveToEndCommand.CanExecute(null), Is.True);
            });

            viewModel.SelectedItem = "D";
            Assert.Multiple(() =>
            {
                Assert.That(viewModel.MoveUpCommand.CanExecute(null), Is.True);
                Assert.That(viewModel.MoveToFirstCommand.CanExecute(null), Is.True);
                Assert.That(viewModel.MoveDownCommand.CanExecute(null), Is.False);
                Assert.That(viewModel.MoveToEndCommand.CanExecute(null), Is.False);
            });

            viewModel.MoveUpCommand.Execute(null);

            Assert.That(viewModel.Items, Is.EqualTo(new[] { "A", "B", "D", "C" }));
        }

        [Test]
        public void MoveUp_ShouldKeepTheSameVisualCollectionInstance()
        {
            var items = viewModel.Items;
            viewModel.SelectedItem = "C";

            viewModel.MoveUp();

            Assert.That(viewModel.Items, Is.SameAs(items));
        }
    }
}
