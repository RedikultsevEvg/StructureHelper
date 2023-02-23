using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Entities.Values.Primitives;
using FieldVisualizer.InfraStructures.Enums;
using FieldVisualizer.Services.ColorServices;
using FieldVisualizer.Services.PrimitiveServices;
using FieldVisualizer.Windows;
using NUnit.Framework;
using System.Collections.Generic;
using System.Windows.Media;

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
