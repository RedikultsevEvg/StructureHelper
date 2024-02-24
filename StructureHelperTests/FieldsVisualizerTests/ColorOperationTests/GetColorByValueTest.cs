using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.ColorMaps.Factories;
using FieldVisualizer.Entities.Values;
using FieldVisualizer.Services.ColorServices;
using NUnit.Framework;

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

        [TestCase(0, 100, -10, 255, 128, 128, 128)] //Gray as less than minimum
        [TestCase(0, 100, 0, 255, 255, 128, 128)]
        [TestCase(0, 100, 50, 255, 255, 254, 0)] //!!!!!!254 cause broading range in method!
        [TestCase(0, 100, 100, 255, 0, 0, 254)]//Blue
        [TestCase(0, 100, 110, 255,128, 128, 128)] //Gray as greater than maximum
        [TestCase(-100, 100, 110, 255, 128, 128, 128)] //Gray as greater than maximum
        [TestCase(-100, 100, 100, 255, 0, 0, 254)]//Blue
        [TestCase(100, 100, -100, 255, 255, 128, 128)]
        public void Run_ShouldPass(double bottomVal, double topVal, double val, int expectedA, int expectedR, int expectedG, int expectedB)
        {
            //Arrange
            valueRange = new ValueRange() { BottomValue = bottomVal, TopValue = topVal };
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
