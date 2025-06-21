using LoaderCalculator.Data.Materials;
using Moq;
using NUnit.Framework;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;
using System;

namespace StructureHelperTests.UnitTests.BeamShearTests
{
        [TestFixture]
        public class GetReducedAreaLogicSP63_2018_rev3Tests
        {
            [TestCase(-500, 0.3, 0.5, 0.001, 0.16999999999999998d)]
            [TestCase(-500, 0.3, 0.5, 0.002, 0.19d)]
            [TestCase(1000, 0.3, 0.5, 0.001, 0.16666666666666666d)]
            [TestCase(1000, 0.3, 0.5, 0.002, 0.18333333333333335d)]
            public void GetArea_ReturnsCorrectResult(double force, double width, double depth, double reinforcementArea, double expectedArea)
            {
                
            // Arrange
                var mockConcreteMaterial = new Mock<IConcreteLibMaterial>();
                mockConcreteMaterial.Setup(m => m.GetLoaderMaterial(It.IsAny<LimitStates>(), It.IsAny<CalcTerms>()))
                    .Returns(new Material { InitModulus = 3e10 });
                mockConcreteMaterial.Setup(m => m.GetStrength(It.IsAny<LimitStates>(), It.IsAny<CalcTerms>()))
                    .Returns((20e6, 1.2e6));

            var mockReinforcementMaterial = new Mock<IReinforcementLibMaterial>();
            mockReinforcementMaterial.Setup(m => m.GetLoaderMaterial(It.IsAny<LimitStates>(), It.IsAny<CalcTerms>()))
                .Returns(new Material { InitModulus = 2e11 });
            mockReinforcementMaterial.Setup(m => m.GetStrength(It.IsAny<LimitStates>(), It.IsAny<CalcTerms>()))
                .Returns((200e6, 200e6));

            var mockSection = new Mock<IBeamShearSection>();
                mockSection.Setup(s => s.ConcreteMaterial).Returns(mockConcreteMaterial.Object);
                mockSection.Setup(s => s.ReinforcementMaterial).Returns(mockReinforcementMaterial.Object);
                mockSection.Setup(s => s.ReinforcementArea).Returns(reinforcementArea);

                var mockInclSection = new Mock<IInclinedSection>();
                mockInclSection.Setup(s => s.WebWidth).Returns(width);
                mockInclSection.Setup(s => s.FullDepth).Returns(depth);
                mockInclSection.Setup(s => s.BeamShearSection).Returns(mockSection.Object);
                mockInclSection.Setup(s => s.LimitState).Returns(LimitStates.ULS);
                mockInclSection.Setup(s => s.CalcTerm).Returns(CalcTerms.ShortTerm);

                var logic = new GetReducedAreaLogicSP63_2018_rev3(force, mockInclSection.Object, null);

                // Act
                var result = logic.GetArea();

                // Assert
                Assert.That(result, Is.EqualTo(expectedArea).Within(1e-6));
            }
        }
    }
