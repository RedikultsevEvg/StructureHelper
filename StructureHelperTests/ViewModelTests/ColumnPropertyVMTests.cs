using Moq;
using NUnit.Framework;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperTests.ViewModelTests
{
    [TestFixture]
    public class ColumnPropertyVMTests
    {
        private Mock<IColumnFileProperty> _mockColumnProperty;

        [SetUp]
        public void SetUp()
        {
            _mockColumnProperty = new Mock<IColumnFileProperty>();
        }

        [Test]
        public void CheckViewModelCreating_RunShouldPass()
        {
            //Arrange
            _mockColumnProperty.Setup(x => x.Name).Returns("TestName");
            _mockColumnProperty.Setup(x => x.SearchingName).Returns("TestSearchName");
            _mockColumnProperty.Setup(x => x.Index).Returns(0);
            _mockColumnProperty.Setup(x => x.Factor).Returns(1);
            //Act
            ColumnFilePropertyVM viewModel = new(_mockColumnProperty.Object);
            //Assert
            Assert.That(viewModel, !Is.Null);
        }
    }
}
