using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearCalculatorViewModel : OkCancelViewModelBase
    {
        private IBeamShearCalculator calculator;
        public BeamShearCalculatorViewModel(IBeamShearCalculator calculator)
        {
            this.calculator = calculator;
        }
    }
}
