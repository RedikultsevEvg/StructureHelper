using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class LimitCurveCalculatorViewModel : OkCancelViewModelBase
    {
        LimitCurvesCalculator calculator;
        public string Name
        {
            get => calculator.Name;
            set
            {
                calculator.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public bool ShowTraceData
        {
            get
            {
                return calculator.ShowTraceData;
            }

            set
            {
                calculator.ShowTraceData = value;
                OnPropertyChanged(nameof(ShowTraceData));
            }
        }

        public LimitCurveDataViewModel LimitCurveDataViewModel { get; }
        public LimitCurveCalculatorViewModel(LimitCurvesCalculator calculator, IEnumerable<INdmPrimitive> allowedPrimitives)
        {
            this.calculator = calculator;
            LimitCurveDataViewModel = new LimitCurveDataViewModel(calculator.InputData, allowedPrimitives);
        }

        public override void OkAction()
        {
            LimitCurveDataViewModel.RefreshInputData();
            base.OkAction();
        }
    }
}
