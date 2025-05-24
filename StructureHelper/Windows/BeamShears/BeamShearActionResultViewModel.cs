using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearActionResultViewModel : ViewModelBase
    {
        private IBeamShearActionResult result;
        public IBeamShearSectionLogicResult SelectedResult { get; set; }
        public List<IBeamShearSectionLogicResult> SectionResults => result.SectionResults;
        public ValidResultCounterVM ValidResultCounter { get; }

        public BeamShearActionResultViewModel(IBeamShearActionResult result)
        {
            this.result = result;
            ValidResultCounter = new(this.result.SectionResults);
        }

    }
}
