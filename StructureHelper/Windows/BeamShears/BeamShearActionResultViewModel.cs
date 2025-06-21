using StructureHelper.Infrastructure;
using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelper.Windows.CalculationWindows.ProgressViews;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearActionResultViewModel : ViewModelBase
    {
        private IBeamShearActionResult result;
        private RelayCommand showTraceCommand;

        public IBeamShearSectionLogicResult SelectedResult { get; set; }
        public List<IBeamShearSectionLogicResult> SectionResults => result.SectionResults;
        public ValidResultCounterVM ValidResultCounter { get; }

        public BeamShearActionResultViewModel(IBeamShearActionResult result)
        {
            this.result = result;
            ValidResultCounter = new(this.result.SectionResults);
        }

        public ICommand ShowTraceCommand => showTraceCommand ??= new RelayCommand(ShowTrace, o => SelectedResult != null);

        private void ShowTrace(object obj)
        {
            if (SelectedResult.TraceLogger is null) { return; }
            var traceWindows = new TraceDocumentView(SelectedResult.TraceLogger);
            traceWindows.ShowDialog();
        }
    }
}
