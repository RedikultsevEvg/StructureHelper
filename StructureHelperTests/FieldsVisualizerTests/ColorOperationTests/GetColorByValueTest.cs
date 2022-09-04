using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.InfraStructures.Enums;
using FieldVisualizer.Services.ColorServices;
using NUnit.Framework;
using System.Windows.Media;

namespace StructureHelperTests.FieldsVisualizerTests.ColorOperationTests
{
    public class GetColorByValueTest
    {
        private IColorMap FullSpectrum;
        private IValueRange valueRange;

        [SetUp]
        public void Setup()
        {
            FullSpectrum = ColorMapFactory.GetColorMap(ColorMapsTypes.FullSpectrum);
            valueRange = new ValueRange() { BottomValue = 0, TopValue = 100 };
        }

        [TestCase(-10, 255, 128, 128, 128)] //Gray as less than minimum
        [TestCase(0, 255, 255, 128, 128)]
        [TestCase(50, 255, 255, 255, 0)]
        [TestCase(100, 255, 0, 0, 255)]//Blue
        [TestCase(110, 255,128, 128, 128)] //Gray as greater than maximum
        public void Run_ShouldPass(double val, int expectedA, int expectedR, int expectedG, int expectedB)
        {
            //Arrange

            //Act
            var result = ColorOperations.GetColorByValue(valueRange, FullSpectrum, val);
            var actualA = result.A;
            var actualR = result.R;
            var actualG = result.G;
            var actualB = result.B;
            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedA, actualA);
            Assert.AreEqual(expectedR, actualR);
            Assert.AreEqual(expectedG, actualG);
            Assert.AreEqual(expectedB, actualB);
        }
    }
}
