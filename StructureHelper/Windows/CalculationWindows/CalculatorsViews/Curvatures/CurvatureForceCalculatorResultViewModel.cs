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
        private ICurvatureForceCalculatorInputData inputData;

        public bool IsValid
        {
            get
            {
                return resultModel.IsValid;
            }
            set
            {
                resultModel.IsValid = value;
            }
        }

        public ICurvatureForceCalculatorInputData InputData
        {
            get => resultModel.InputData;
        }

        public ICurvatureForceCalculatorResult ForceCalculatorResult => resultModel;

        public CurvatureForceCalculatorResultViewModel(ICurvatureForceCalculatorResult resultModel)
        {
            this.resultModel = resultModel;
        }
    }
}
