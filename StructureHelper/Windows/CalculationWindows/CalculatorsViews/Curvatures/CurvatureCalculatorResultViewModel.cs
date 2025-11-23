using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    public class CurvatureCalculatorResultViewModel : ViewModelBase
    {
        private readonly ICurvatureCalculatorResult resultModel;
        public CurvatureForceCalculatorResultViewModel SelectedResult {  get; set; }
        public List<CurvatureForceCalculatorResultViewModel> ForcesResult { get; set; }
        public ICurvatureCalculatorResult Result => resultModel;
        public ValidResultCounterVM ValidResultCounter { get; }

        public CurvatureCalculatorResultViewModel(ICurvatureCalculatorResult resultModel)
        {
            this.resultModel = resultModel;
            ValidResultCounter = new(this.resultModel.ForceCalculatorResults);
            ForcesResult = new();
            foreach (var item in resultModel.ForceCalculatorResults)
            {
                ForcesResult.Add(new CurvatureForceCalculatorResultViewModel(item));
            }
        }

    }
}
