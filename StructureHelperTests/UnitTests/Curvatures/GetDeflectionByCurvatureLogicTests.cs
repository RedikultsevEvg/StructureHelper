using Moq;
using NUnit.Framework;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace StructureHelperTests.UnitTests.Curvatures
{


    [TestFixture]
    public class GetDeflectionByCurvatureLogicTests
    {
        private Mock<IShiftTraceLogger> _logger;
        private GetDeflectionByCurvatureLogic _logic;
        private Mock<IForceTuple> _curvature;
        private Mock<IDeflectionFactor> _factor;
        private Mock<IForceTuple> _k;
        private Mock<IForceTuple> _max;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<IShiftTraceLogger>();
            _logic = new GetDeflectionByCurvatureLogic(_logger.Object);

            _curvature = new Mock<IForceTuple>();
            _factor = new Mock<IDeflectionFactor>();
            _k = new Mock<IForceTuple>();
            _max = new Mock<IForceTuple>();

            _factor.Setup(f => f.DeflectionFactors).Returns(_k.Object);
            _factor.Setup(f => f.MaxDeflections).Returns(_max.Object);
        }

        [Test]
        public void GetDeflection_ComputesDeflectionMxCorrectly()
        {
            _factor.Setup(f => f.SpanLength).Returns(5.0);
            _k.Setup(f => f.Mx).Returns(2.0);
            _curvature.Setup(c => c.Mx).Returns(3.0);
            _max.Setup(m => m.Mx).Returns(10.0);

            var result = _logic.GetDeflection(_curvature.Object, _factor.Object);

            // Expected: k * curvature * L^2 = 2 * 3 * 25 = 150
            Assert.That(result.DeflectionMx.Deflection, Is.EqualTo(150.0));
            Assert.That(result.DeflectionMx.Curvature, Is.EqualTo(3.0));
            Assert.That(result.DeflectionMx.UltimateDeflection, Is.EqualTo(10.0));
        }

        [Test]
        public void GetDeflection_ComputesDeflectionMyCorrectly()
        {
            _factor.Setup(f => f.SpanLength).Returns(4.0);
            _k.Setup(f => f.My).Returns(1.5);
            _curvature.Setup(c => c.My).Returns(2.0);
            _max.Setup(m => m.My).Returns(7.0);

            var result = _logic.GetDeflection(_curvature.Object, _factor.Object);

            // Expected: 1.5 * 2 * 16 = 48
            Assert.That(result.DeflectionMy.Deflection, Is.EqualTo(48.0));
            Assert.That(result.DeflectionMy.Curvature, Is.EqualTo(2.0));
            Assert.That(result.DeflectionMy.UltimateDeflection, Is.EqualTo(7.0));
        }

        [Test]
        public void GetDeflection_ComputesDeflectionNzCorrectly()
        {
            _factor.Setup(f => f.SpanLength).Returns(10.0);
            _k.Setup(f => f.Nz).Returns(1.2);
            _curvature.Setup(c => c.Nz).Returns(4.0);
            _max.Setup(m => m.Nz).Returns(5.0);

            var result = _logic.GetDeflection(_curvature.Object, _factor.Object);

            // Expected: 1.2 * 4 * 10 = 48
            Assert.That(result.DeflectionNz.Deflection, Is.EqualTo(48.0));
            Assert.That(result.DeflectionNz.Curvature, Is.EqualTo(4.0));
            Assert.That(result.DeflectionNz.UltimateDeflection, Is.EqualTo(5.0));
        }

        [Test]
        public void GetDeflection_LogsSpanLength()
        {
            _factor.Setup(f => f.SpanLength).Returns(6.0);

            _logic.GetDeflection(_curvature.Object, _factor.Object);

            _logger.Verify(l => l.AddMessage("Span length L = 6(m)"), Times.Once);
        }

        [Test]
        public void GetDeflection_LogsAllDeflectionsInCorrectFormat()
        {
            _factor.Setup(f => f.SpanLength).Returns(3.0);
            _k.Setup(f => f.Mx).Returns(2);
            _k.Setup(f => f.My).Returns(3);
            _k.Setup(f => f.Nz).Returns(4);

            _curvature.Setup(c => c.Mx).Returns(1.0);
            _curvature.Setup(c => c.My).Returns(2.0);
            _curvature.Setup(c => c.Nz).Returns(3.0);

            _max.Setup(m => m.Mx).Returns(0);
            _max.Setup(m => m.My).Returns(0);
            _max.Setup(m => m.Nz).Returns(0);

            _logic.GetDeflection(_curvature.Object, _factor.Object);

            _logger.Verify(l =>
                l.AddMessage("Deflection along X axis = 2 * 1 * (3)^2 = 18(m)"),
                Times.Once);

            _logger.Verify(l =>
                l.AddMessage("Deflection along Y axis = 3 * 2 * (3)^2 = 54(m)"),
                Times.Once);

            _logger.Verify(l =>
                l.AddMessage("Deflection along Z axis = 4 * 3 * 3 = 36(m)"),
                Times.Once);
        }

        [Test]
        public void GetDeflection_ZeroCurvature_ProducesZeroDeflection()
        {
            _factor.Setup(f => f.SpanLength).Returns(10);
            _k.Setup(f => f.Mx).Returns(5);
            _curvature.Setup(c => c.Mx).Returns(0);

            var result = _logic.GetDeflection(_curvature.Object, _factor.Object);

            Assert.That(result.DeflectionMx.Deflection, Is.EqualTo(0));
        }
    }

}
