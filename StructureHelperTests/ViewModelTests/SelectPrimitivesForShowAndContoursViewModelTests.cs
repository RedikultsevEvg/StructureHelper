using NUnit.Framework;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class SelectPrimitivesForShowAndContoursViewModelTests
    {
        [Test]
        public void Constructor_ShouldCreateTwoIndependentSelections()
        {
            var primitives = new List<INdmPrimitive>
            {
                new PointNdmPrimitive(),
                new PointNdmPrimitive()
            };

            var viewModel = new SelectPrimitivesForShowAndContoursViewModel(primitives);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.ShowItems, Is.Not.Null);
                Assert.That(viewModel.ContoursItems, Is.Not.Null);
                Assert.That(viewModel.ShowItems, Is.Not.SameAs(viewModel.ContoursItems));
                Assert.That(viewModel.ShowItems.CollectionItems, Has.Count.EqualTo(2));
                Assert.That(viewModel.ContoursItems.CollectionItems, Has.Count.EqualTo(2));
            });
        }

        [Test]
        public void Selections_ShouldBeIndependent()
        {
            var primitives = new List<INdmPrimitive>
            {
                new PointNdmPrimitive(),
                new PointNdmPrimitive()
            };
            var viewModel = new SelectPrimitivesForShowAndContoursViewModel(primitives);

            viewModel.ShowItems.SetIsSelected(false);

            Assert.Multiple(() =>
            {
                Assert.That(viewModel.ShowItems.SelectedCount, Is.Zero);
                Assert.That(viewModel.ContoursItems.SelectedCount, Is.EqualTo(2));
            });
        }
    }
}
