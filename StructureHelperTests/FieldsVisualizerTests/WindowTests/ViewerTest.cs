using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Windows;
using NUnit.Framework;

namespace StructureHelperTests.FieldsVisualizerTests.WindowTests
{
    public class ViewerTest
    {
        private IEnumerable<IPrimitiveSet> primitiveSets;

        [SetUp]
        public void Setup()
        {
            primitiveSets = TestPrimitivesOperation.AddTestPrimitives();
        }
        public void Run_ShouldPass()
        {
            //Arrange
            //Act
            WndFieldViewer viewer = new WndFieldViewer(primitiveSets);
            //Assert
            Assert.NotNull(viewer);
        }

    }
}
