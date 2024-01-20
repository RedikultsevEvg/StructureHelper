using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;


//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.


namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForceCalculatorViewModel : OkCancelViewModelBase
    {
        ForceCalculator forcesCalculator;
        SecondOrderViewModel secondOrderViewModel;

        public string Name
        {
            get { return forcesCalculator.Name; }
            set { forcesCalculator.Name = value; }
        }

        public double IterationAccuracy
        {
            get { return forcesCalculator.Accuracy.IterationAccuracy; }
            set { forcesCalculator.Accuracy.IterationAccuracy = value;}
        }

        public int MaxIterationCount
        {
            get { return forcesCalculator.Accuracy.MaxIterationCount; }
            set { forcesCalculator.Accuracy.MaxIterationCount = value; }
        }

        public SecondOrderViewModel SecondOrder => secondOrderViewModel;

        public bool ULS { get; set; }
        public bool SLS { get; set; }
        public bool ShortTerm { get; set; }
        public bool LongTerm { get; set; }

        public SourceTargetVM<IForceAction> CombinationViewModel { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; private set; }

        public ForceCalculatorViewModel(IEnumerable<INdmPrimitive> allowedPrimitives, IEnumerable<IForceAction> allowedCombinations, ForceCalculator forcesCalculator)
        {
            this.forcesCalculator = forcesCalculator;
            secondOrderViewModel = new SecondOrderViewModel(this.forcesCalculator.CompressedMember);
            CombinationViewModel = SourceTargetFactory.GetSourceTargetVM(allowedCombinations, this.forcesCalculator.ForceActions);
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(allowedPrimitives, this.forcesCalculator.Primitives);
            InputRefresh();
        }

        public void InputRefresh()
        {
            ULS = forcesCalculator.LimitStatesList.Contains(LimitStates.ULS);
            SLS = forcesCalculator.LimitStatesList.Contains(LimitStates.SLS);
            ShortTerm = forcesCalculator.CalcTermsList.Contains(CalcTerms.ShortTerm);
            LongTerm = forcesCalculator.CalcTermsList.Contains(CalcTerms.LongTerm);
        }

        public void Refresh()
        {
            var combinations = CombinationViewModel.GetTargetItems();
            forcesCalculator.ForceActions.Clear();
            foreach (var item in combinations)
            {
                forcesCalculator.ForceActions.Add(item);
            }
            forcesCalculator.Primitives.Clear();
            foreach (var item in PrimitivesViewModel.GetTargetItems())
            {
                forcesCalculator.Primitives.Add(item.GetNdmPrimitive());
            }
            forcesCalculator.LimitStatesList.Clear();
            if (ULS == true) { forcesCalculator.LimitStatesList.Add(LimitStates.ULS); }
            if (SLS == true) { forcesCalculator.LimitStatesList.Add(LimitStates.SLS); }
            forcesCalculator.CalcTermsList.Clear();
            if (ShortTerm == true) { forcesCalculator.CalcTermsList.Add(CalcTerms.ShortTerm); }
            if (LongTerm == true) { forcesCalculator.CalcTermsList.Add(CalcTerms.LongTerm); }
        }
    }
}
