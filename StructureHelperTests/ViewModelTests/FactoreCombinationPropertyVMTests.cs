
using Moq;
using NUnit.Framework;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class FactoreCombinationPropertyVMTests
    {
        private Mock<IFactoredCombinationProperty> _mockModel;

        [SetUp]
        public void SetUp()
        {
            _mockModel = new Mock<IFactoredCombinationProperty>();
        }

        [TestCase(LimitStates.SLS, CalcTerms.ShortTerm, 0.9, 1.2)]
        [TestCase(LimitStates.ULS, CalcTerms.ShortTerm, 0.9, 1.2)]
        [TestCase(LimitStates.ULS, CalcTerms.LongTerm, 0.9, 1.2)]
        public void CreateViewModel_RunShouldPass(LimitStates limitStates, CalcTerms calcTerms, double longTermFactor, double ulsFactor)
        {
            //Arrange
            _mockModel.Setup(x => x.LimitState).Returns(limitStates);
            _mockModel.Setup(x => x.CalcTerm).Returns(calcTerms);
            _mockModel.Setup(x => x.LongTermFactor).Returns(longTermFactor);
            _mockModel.Setup(x => x.ULSFactor).Returns(ulsFactor);
            //Act
            FactoredCombinationPropertyVM viewModel = new(_mockModel.Object);
            //Assert 
            Assert.That(viewModel, !Is.Null);
            Assert.That(viewModel.LimitState, Is.EqualTo(limitStates));
            Assert.That(viewModel.CalcTerm, Is.EqualTo(calcTerms));
            Assert.That(viewModel.LongTermFactor, Is.EqualTo(longTermFactor));
            Assert.That(viewModel.ULSFactor, Is.EqualTo(ulsFactor));
        }
    }
}
