using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
    public class GetVerticalTShapeFactorSHLogicTests
    {
        private Mock<IInclinedSection> _inclinedSectionMock = null!;
        private Mock<IVerticalTShape> _verticalTShapeMock = null!;

        [SetUp]
        public void SetUp()
        {
            _inclinedSectionMock = new Mock<IInclinedSection>();
            _verticalTShapeMock = new Mock<IVerticalTShape>();

            // Default geometry:
            // bw = 0.2 m
            // bf = 0.6 m
            // hf = 0.1 m
            // d  = 0.5 m
            // c  = 1.5 m

            ConfigureShape(
                webWidth: 0.2,
                flangeWidth: 0.6,
                flangeHeight: 0.1,
                flangeOffset: 0);

            ConfigureSection(
                effectiveDepth: 0.5,
                startCoord: 0,
                endCoord: 1.5);
        }

        private void ConfigureShape(
            double webWidth,
            double flangeWidth,
            double flangeHeight,
            double flangeOffset)
        {
            _verticalTShapeMock.SetupGet(x => x.WebWidth)
                .Returns(webWidth);
            _verticalTShapeMock.SetupGet(x => x.FlangeWidth)
                .Returns(flangeWidth);
            _verticalTShapeMock.SetupGet(x => x.FlangeHeight)
                .Returns(flangeHeight);
            _verticalTShapeMock.SetupGet(x => x.FlangeXOffset)
                .Returns(flangeOffset);
        }

        private void ConfigureSection(
            double effectiveDepth,
            double startCoord,
            double endCoord)
        {
            _inclinedSectionMock.SetupGet(x => x.EffectiveDepth)
                .Returns(effectiveDepth);
            _inclinedSectionMock.SetupGet(x => x.StartCoord)
                .Returns(startCoord);
            _inclinedSectionMock.SetupGet(x => x.EndCoord)
                .Returns(endCoord);
        }

        private GetVerticalTShapeFactorSHLogic CreateSut()
        {
            return new GetVerticalTShapeFactorSHLogic(
                _inclinedSectionMock.Object,
                _verticalTShapeMock.Object);
        }

        [Test]
        public void Constructor_NullInclinedSection_ThrowsException()
        {
            Assert.Throws<StructureHelperException>(() =>
                new GetVerticalTShapeFactorSHLogic(
                    null!,
                    _verticalTShapeMock.Object));
        }

        [Test]
        public void Constructor_NullVerticalTShape_ThrowsException()
        {
            Assert.Throws<StructureHelperException>(() =>
                new GetVerticalTShapeFactorSHLogic(
                    _inclinedSectionMock.Object,
                    null!));
        }

        [Test]
        public void GetShapeFactor_ShearSpanBelowMinimum_ReturnsOne()
        {
            // c/d = 1.0 < 1.5
            ConfigureSection(0.5, 0, 0.5);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.0).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_ShearSpanAtMinimum_ReturnsOne()
        {
            // c/d = 1.5
            ConfigureSection(0.5, 0, 0.75);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.0).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_ShearSpanAtMaximum_UsesFullFlangeArea()
        {
            // c/d = 3.0
            // Each cantilever = 0.2 m
            // Each area = 0.2*0.1 - 0.2²/3*0.5
            //           = 0.0133333333 m²
            // Total flange area = 0.0266666667 m²
            // Web area = 0.2*0.5 = 0.1 m²
            // Raw factor = 1.2666667
            // Limited to 1.25

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.25).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_ShearSpanAboveMaximum_UsesFullFlangeArea()
        {
            ConfigureSection(0.5, 0, 2.0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.25).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_IntermediateShearSpan_InterpolatesLinearly()
        {
            // c/d = 2.0
            // Interpolation ratio = (2.0-1.5)/(3.0-1.5) = 1/3
            // Effective flange area = 0.0266666667 / 3
            // Factor = 1 + 0.0088888889 / 0.1

            ConfigureSection(0.5, 0, 1.0);

            double expected = 1.088888888888889;

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_FlangeHeightBelowMinimum_ReturnsOne()
        {
            // hf < 0.1*d = 0.05
            ConfigureShape(0.2, 0.6, 0.04, 0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.0).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_FlangeHeightAtMinimum_IsConsidered()
        {
            // hf = 0.05
            // Each cantilever = 0.2 > 3*0.05
            // Each area = 3*0.05²*0.5 = 0.00375
            // Total area = 0.0075
            // Factor = 1 + 0.0075/0.1 = 1.075

            ConfigureShape(0.2, 0.6, 0.05, 0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.075).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_FlangeCantileverAboveLimit_LimitsEffectiveWidth()
        {
            // hf = 0.05
            // Maximum effective cantilever = 3*hf = 0.15
            // Actual cantilever = 0.4
            // Each area = 3*0.05²*0.5 = 0.00375

            ConfigureShape(0.2, 1.0, 0.05, 0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.075).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_AsymmetricFlange_CalculatesBothSides()
        {
            // bw = 0.2, bf = 0.5, offset = 0.05
            // Left cantilever = 0.10
            // Right cantilever = 0.20
            // hf = 0.1
            //
            // Left area = 0.1*0.1 - 0.1²/6
            //           = 0.0083333333
            // Right area = 0.2*0.1 - 0.2²/6
            //            = 0.0133333333
            // Total = 0.0216666667
            // Factor = 1.2166666667

            ConfigureShape(0.2, 0.5, 0.1, 0.05);

            double expected = 1.216666666666667;

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_FlangeWidthEqualsWebWidth_ReturnsOne()
        {
            ConfigureShape(0.2, 0.2, 0.1, 0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.0).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_NegativeLeftCantilever_ThrowsException()
        {
            // (0.4-0.2)/2 - 0.15 = -0.05
            ConfigureShape(0.2, 0.4, 0.1, 0.15);

            Assert.Throws<StructureHelperException>(() =>
                CreateSut().GetShapeFactor());
        }

        [Test]
        public void GetShapeFactor_NegativeRightCantilever_ThrowsException()
        {
            ConfigureShape(0.2, 0.4, 0.1, -0.15);

            Assert.Throws<StructureHelperException>(() =>
                CreateSut().GetShapeFactor());
        }

        [Test]
        public void GetShapeFactor_LargeFlangeArea_CapsFactorAt125()
        {
            ConfigureShape(0.1, 1.0, 0.3, 0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(1.25).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_NonzeroStartCoord_UsesCoordinateDifference()
        {
            // c = 2.0 - 1.0 = 1.0
            // c/d = 2.0
            ConfigureSection(0.5, 1.0, 2.0);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result,
                Is.EqualTo(1.088888888888889).Within(1e-10));
        }

        [Test]
        public void GetShapeFactor_CalledTwice_ReturnsSameResult()
        {
            var sut = CreateSut();

            double first = sut.GetShapeFactor();
            double second = sut.GetShapeFactor();

            Assert.Multiple(() =>
            {
                Assert.That(first, Is.EqualTo(1.25).Within(1e-10));
                Assert.That(second, Is.EqualTo(first).Within(1e-10));
            });
        }

        [TestCase(0.50, 1.0)]
        [TestCase(0.75, 1.0)]
        [TestCase(0.90, 1.053333333333333)]
        [TestCase(1.00, 1.088888888888889)]
        [TestCase(1.125, 1.133333333333333)]
        [TestCase(1.50, 1.25)]
        [TestCase(2.00, 1.25)]
        public void GetShapeFactor_DifferentShearSpans_ReturnsExpected(
    double shearSpan,
    double expected)
        {
            ConfigureSection(0.5, 0, shearSpan);

            double result = CreateSut().GetShapeFactor();

            Assert.That(result, Is.EqualTo(expected).Within(1e-10));
        }
    }
}
