using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.BeamShears.Logics;
using System;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
        [TestFixture]
        public class GetLongitudinalForceFactorLogicTests
        {
            [Test]
            public void GetFactor_When_LongitudinalForceIsZero_ReturnsOne()
            {
                var traceLogger = new Mock<IShiftTraceLogger>();
                var inclinedSection = new Mock<IInclinedSection>();
                var mockCheckLogic = new Mock<ICheckEntityLogic<IInclinedSection>>();
                mockCheckLogic.Setup(x => x.Check()).Returns(true);
            List<ITraceLoggerEntry> traceLoggerEntries = new();
            traceLogger.Setup(x => x.TraceLoggerEntries).Returns(traceLoggerEntries);

            var mockReducedAreaLogic = new Mock<IGetReducedAreaLogic>();
                var logic = new GetLongitudinalForceFactorLogic(mockCheckLogic.Object, mockReducedAreaLogic.Object, traceLogger.Object)
                {
                    InclinedSection = inclinedSection.Object,
                    LongitudinalForce = 0
                };

                var result = logic.GetFactor();

                Assert.That(result, Is.EqualTo(1));
            }

            [Test]
            public void GetFactor_When_CheckFails_ThrowsStructureHelperException()
            {
                var traceLogger = new Mock<IShiftTraceLogger>();
                var inclinedSection = new Mock<IInclinedSection>();
                var mockCheckLogic = new Mock<ICheckEntityLogic<IInclinedSection>>();
            List<ITraceLoggerEntry> traceLoggerEntries = new();
            traceLogger.Setup(x => x.TraceLoggerEntries).Returns(traceLoggerEntries);

            mockCheckLogic.Setup(x => x.Check()).Returns(false);
                mockCheckLogic.Setup(x => x.CheckResult).Returns("Check failed");

                var mockReducedAreaLogic = new Mock<IGetReducedAreaLogic>();
                var logic = new GetLongitudinalForceFactorLogic(mockCheckLogic.Object, mockReducedAreaLogic.Object, traceLogger.Object)
                {
                    InclinedSection = inclinedSection.Object,
                    LongitudinalForce = 1000
                };

                Assert.That(() => logic.GetFactor(), Throws.TypeOf<StructureHelperException>());
            }

            [TestCase(-1, 1.0000000333333334d)] //1N
            [TestCase(-1000000, 1.0333333333333334d)] //1MN
            [TestCase(-10000000, 1.25d)] //10MN
            [TestCase(-20000000, 0.83333333333333348d)] //20MN
            [TestCase(-100000000, 0)] //100MN
            public void GetFactor_When_Compression_ReturnsExpectedResult(double force, double expectedFactor)
            {
                var traceLogger = new Mock<IShiftTraceLogger>();
                var inclinedSection = new Mock<IInclinedSection>();
                var mockCheckLogic = new Mock<ICheckEntityLogic<IInclinedSection>>();
                var mockReducedAreaLogic = new Mock<IGetReducedAreaLogic>();
            List<ITraceLoggerEntry> traceLoggerEntries = new();
            traceLogger.Setup(x => x.TraceLoggerEntries).Returns(traceLoggerEntries);

            mockCheckLogic.Setup(x => x.Check()).Returns(true);
                mockReducedAreaLogic.Setup(x => x.GetArea()).Returns(1.0);

                inclinedSection.Setup(x => x.ConcreteCompressionStrength).Returns(30e6);

                var logic = new GetLongitudinalForceFactorLogic(mockCheckLogic.Object, mockReducedAreaLogic.Object, traceLogger.Object)
                {
                    InclinedSection = inclinedSection.Object,
                    LongitudinalForce = force
                };

                var result = logic.GetFactor();

                Assert.That(result, Is.EqualTo(expectedFactor).Within(1e-6));
            }

            [Test]
            public void GetFactor_When_Tension_ReturnsExpectedResult()
            {
                var traceLogger = new Mock<IShiftTraceLogger>();
                var inclinedSection = new Mock<IInclinedSection>();
                var mockCheckLogic = new Mock<ICheckEntityLogic<IInclinedSection>>();
                var mockReducedAreaLogic = new Mock<IGetReducedAreaLogic>();
            List<ITraceLoggerEntry> traceLoggerEntries = new();
            traceLogger.Setup(x => x.TraceLoggerEntries).Returns(traceLoggerEntries);

                mockCheckLogic.Setup(x => x.Check()).Returns(true);
                mockReducedAreaLogic.Setup(x => x.GetArea()).Returns(1.0);

                inclinedSection.Setup(x => x.ConcreteTensionStrength).Returns(3e6);

                var logic = new GetLongitudinalForceFactorLogic(mockCheckLogic.Object, mockReducedAreaLogic.Object, traceLogger.Object)
                {
                    InclinedSection = inclinedSection.Object,
                    LongitudinalForce = 1000000 // +1MN
                };

                var result = logic.GetFactor();

                Assert.That(result, Is.GreaterThanOrEqualTo(0));
            }
        }
    }
