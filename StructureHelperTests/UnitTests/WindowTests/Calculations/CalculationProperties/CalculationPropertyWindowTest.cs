using NUnit.Framework;
using StructureHelper.Windows.CalculationWindows.CalculationPropertyWindow;
using StructureHelper.Windows.ViewModels.Calculations.CalculationProperies;
using StructureHelper.Windows.ViewModels.Calculations.CalculationResult;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace StructureHelperTests.UnitTests.WindowTests.Calculations.CalculationProperties
{
    public class CalculationPropertyWindowTest
    {
        private CalculationPropertyViewModel viewModel;
        private ICalculationProperty calculationProperty;

        [SetUp]
        public void Setup()
        {
            calculationProperty = new CalculationProperty();
            viewModel = new CalculationPropertyViewModel(calculationProperty);
        }

        public void Run_Shoulpass()
        {
            //Arrange

            //Act
            var wnd = new CalculationPropertyView(viewModel);
            //Assert
            Assert.NotNull(wnd);
        }
    }
}
