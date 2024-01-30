using StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic;
using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.LimitCurve;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
