using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;

namespace StructureHelperTests.UnitTests.BeamShearTests
{


    [TestFixture]
    public class StirrupByUniformRebarToDensityConvertStrategyTests
    {
        private Mock<IUpdateStrategy<IStirrup>> _mockUpdateStrategy;
        private Mock<IShiftTraceLogger> _mockTraceLogger;
        private StirrupByRebarToDensityConvertStrategy _convertStrategy;

        [SetUp]
        public void Setup()
        {
            _mockUpdateStrategy = new Mock<IUpdateStrategy<IStirrup>>();
            _mockTraceLogger = new Mock<IShiftTraceLogger>();

            _convertStrategy = new StirrupByRebarToDensityConvertStrategy(
                _mockUpdateStrategy.Object,
                _mockTraceLogger.Object
            );
        }

        [Test]
        public void Convert_UpdatesAndCalculatesDensityCorrectly()
        {
            // Arrange
            var mockMaterial = new Mock<IReinforcementLibMaterial>();
            mockMaterial.Setup(m => m.GetStrength(LimitStates.ULS, CalcTerms.ShortTerm)).Returns((2e8, 2e8));

            var stirrupRebar = new Mock<IStirrupByRebar>();
            stirrupRebar.Setup(s => s.Diameter).Returns(0.02);
            stirrupRebar.Setup(s => s.Material).Returns(mockMaterial.Object);
            stirrupRebar.Setup(s => s.LegCount).Returns(2);
            stirrupRebar.Setup(s => s.Spacing).Returns(0.15);

            // Act
            var result = _convertStrategy.Convert(stirrupRebar.Object);

            // Assert
            _mockUpdateStrategy.Verify(u => u.Update(It.IsAny<IStirrupByDensity>(), stirrupRebar.Object), Times.Once);
            Assert.That(result.StirrupDensity, Is.EqualTo(837758.04095727834d).Within(0.00001));
            //_mockTraceLogger.Verify(t => t.AddMessage(It.IsAny<string>(), It.IsAny<TraceLogStatuses>()), Times.AtLeastOnce);
        }
    }

}
