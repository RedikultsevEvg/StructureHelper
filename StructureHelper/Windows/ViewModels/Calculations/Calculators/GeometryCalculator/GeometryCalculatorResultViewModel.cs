using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators.GeometryCalculator
{
    internal class GeometryCalculatorResultViewModel : ViewModelBase
    {
        private List<ITextParameter> textParameters;

        public List<ITextParameter> TextParameters
        { get => textParameters;
            
        }
        public GeometryCalculatorResultViewModel(List<ITextParameter> textParameters)
        {
            this.textParameters = textParameters;
        }
    }
}
