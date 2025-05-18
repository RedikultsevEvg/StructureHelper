using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearResultViewModel : ViewModelBase
    {
        private IBeamShearCalculatorResult result;
        public IBeamShearActionResult SelectedResult { get; set; }
        public List<IBeamShearActionResult> ActionResults => result.ActionResults;
        public ValidResultCounterVM ValidResultCounter { get; }

        public BeamShearResultViewModel(IBeamShearCalculatorResult result)
        {
            this.result = result;
            ValidResultCounter = new(this.result.ActionResults);
        }

    }
}
