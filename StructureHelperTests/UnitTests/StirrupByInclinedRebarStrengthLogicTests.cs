using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.UnitTests
{

    [TestFixture]
    public class StirrupByInclinedRebarStrengthLogicTests
    {
        private Mock<IInclinedSection> sectionMock;
        private Mock<IStirrupByInclinedRebar> rebarMock;
        private Mock<IRebarSectionStrengthLogic> rebarStrengthMock;
        private Mock<IInterpolateValueLogic> interpolateMock;
        private Mock<IShiftTraceLogger> loggerMock;

        [SetUp]
        public void SetUp()
        {
            sectionMock = new Mock<IInclinedSection>();
            rebarMock = new Mock<IStirrupByInclinedRebar>();
            rebarStrengthMock = new Mock<IRebarSectionStrengthLogic>();
            interpolateMock = new Mock<IInterpolateValueLogic>();
            loggerMock = new Mock<IShiftTraceLogger>();

            // Default geometry
            rebarMock.SetupGet(r => r.TransferLength).Returns(0.1);
            rebarMock.SetupGet(r => r.StartCoordinate).Returns(0.0);
            rebarMock.SetupGet(r => r.AngleOfInclination).Returns(45.0);
            rebarMock.SetupGet(r => r.CompressedGap).Returns(0.0);
            rebarMock.SetupGet(r => r.LegCount).Returns(2);
            rebarMock.SetupGet(r => r.RebarSection).Returns(Mock.Of<IRebarSection>());

            sectionMock.SetupGet(s => s.EffectiveDepth).Returns(1.0);

            rebarStrengthMock.Setup(m => m.GetRebarMaxTensileForce()).Returns(1000.0);
            interpolateMock.Setup(m => m.GetValueY()).Returns(123.0);
        }

        [Test]
        public void GetShearStrength_RebarOutsideSectionStart_ReturnsZero()
        {
            sectionMock.SetupGet(s => s.StartCoord).Returns(10.0); // start is after rebar end
            sectionMock.SetupGet(s => s.EndCoord).Returns(12.0);

            var logic = CreateLogic();
            var result = logic.GetShearStrength();

            Assert.That(result, Is.EqualTo(0.0));
            loggerMock.Verify(l => l.AddMessage(It.Is<string>(msg => msg.Contains("has been ignored"))));
        }

        [Test]
        public void GetShearStrength_RebarOutsideSectionEnd_ReturnsZero()
        {
            sectionMock.SetupGet(s => s.StartCoord).Returns(-5.0);
            sectionMock.SetupGet(s => s.EndCoord).Returns(-1.0);

            var logic = CreateLogic();
            var result = logic.GetShearStrength();

            Assert.That(result, Is.EqualTo(0.0));
            loggerMock.Verify(l => l.AddMessage(It.Is<string>(msg => msg.Contains("has been ignored"))));
        }

        [Test]
        public void GetShearStrength_StartTransferZone_UsesInterpolation()
        {
            sectionMock.SetupGet(s => s.StartCoord).Returns(0.0);
            sectionMock.SetupGet(s => s.EndCoord).Returns(0.05); // falls in start transfer zone

            var logic = CreateLogic();
            var result = logic.GetShearStrength();

            Assert.That(result, Is.EqualTo(123.0)); // from interpolateMock
            interpolateMock.Verify(m => m.GetValueY(), Times.Once);
        }

        [Test]
        public void GetShearStrength_EndTransferZone_UsesInterpolation()
        {
            sectionMock.SetupGet(s => s.StartCoord).Returns(0.95); // falls in end transfer zone
            sectionMock.SetupGet(s => s.EndCoord).Returns(2.0);

            var logic = CreateLogic();
            var result = logic.GetShearStrength();

            Assert.That(result, Is.EqualTo(123.0));
            interpolateMock.Verify(m => m.GetValueY(), Times.Once);
        }

        [Test]
        public void GetShearStrength_MainZone_ComputesStrength()
        {
            sectionMock.SetupGet(s => s.StartCoord).Returns(0.2);
            sectionMock.SetupGet(s => s.EndCoord).Returns(1.0);

            var logic = CreateLogic();
            var result = logic.GetShearStrength();

            // Strength = 0.75 * 1000 * sin(45°) * 2
            var expected = 0.75 * 1000.0 * Math.Sin(Math.PI / 4) * 2;
            Assert.That(result, Is.EqualTo(expected).Within(1e-6));
        }

        [Test]
        public void GetShearStrength_TransferZonesOverlap_ThrowsException()
        {
            rebarMock.SetupGet(r => r.TransferLength).Returns(5.0); // huge transfer length

            var logic = CreateLogic();
            Assert.Throws<StructureHelperException>(() => logic.GetShearStrength());
        }

        private StirrupByInclinedRebarStrengthLogic CreateLogic()
        {
            return new StirrupByInclinedRebarStrengthLogic(
                sectionMock.Object,
                rebarMock.Object,
                rebarStrengthMock.Object,
                interpolateMock.Object,
                loggerMock.Object
            );
        }
    }
}
