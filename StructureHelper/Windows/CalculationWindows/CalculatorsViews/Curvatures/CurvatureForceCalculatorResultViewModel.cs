using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    public class CurvatureForceCalculatorResultViewModel : ViewModelBase
    {
        private readonly ICurvatureForceCalculatorResult resultModel;

        public CurvatureForceCalculatorResultViewModel(ICurvatureForceCalculatorResult resultModel)
        {
            this.resultModel = resultModel;
        }
    }
}
