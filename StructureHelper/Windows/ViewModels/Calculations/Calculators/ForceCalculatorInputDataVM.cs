using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class ForceCalculatorInputDataVM : ViewModelBase
    {
        private IForceCalculatorInputData inputData;
        SecondOrderViewModel secondOrderViewModel;

        public double IterationAccuracy
        {
            get { return inputData.Accuracy.IterationAccuracy; }
            set { inputData.Accuracy.IterationAccuracy = value; }
        }

        public int MaxIterationCount
        {
            get { return inputData.Accuracy.MaxIterationCount; }
            set { inputData.Accuracy.MaxIterationCount = value; }
        }

        public SecondOrderViewModel SecondOrder => secondOrderViewModel;

        public bool ULS { get; set; }
        public bool SLS { get; set; }
        public bool ShortTerm { get; set; }
        public bool LongTerm { get; set; }

        public SourceTargetVM<IForceAction> CombinationViewModel { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; private set; }


        public ForceCalculatorInputDataVM(IForceCalculatorInputData inputData, IEnumerable<INdmPrimitive> allowedPrimitives, IEnumerable<IForceAction> allowedCombinations)
        {
            this.inputData = inputData;
            secondOrderViewModel = new SecondOrderViewModel(this.inputData.CompressedMember);
            CombinationViewModel = SourceTargetFactory.GetSourceTargetVM(allowedCombinations, this.inputData.ForceActions);
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(allowedPrimitives, this.inputData.Primitives);
            InputRefresh();
        }

        public void InputRefresh()
        {
            ULS = inputData.LimitStatesList.Contains(LimitStates.ULS);
            SLS = inputData.LimitStatesList.Contains(LimitStates.SLS);
            ShortTerm = inputData.CalcTermsList.Contains(CalcTerms.ShortTerm);
            LongTerm = inputData.CalcTermsList.Contains(CalcTerms.LongTerm);
        }

        public void Refresh()
        {
            var combinations = CombinationViewModel.GetTargetItems();
            inputData.ForceActions.Clear();
            foreach (var item in combinations)
            {
                inputData.ForceActions.Add(item);
            }
            inputData.Primitives.Clear();
            foreach (var item in PrimitivesViewModel.GetTargetItems())
            {
                inputData.Primitives.Add(item.GetNdmPrimitive());
            }
            inputData.LimitStatesList.Clear();
            if (ULS == true) { inputData.LimitStatesList.Add(LimitStates.ULS); }
            if (SLS == true) { inputData.LimitStatesList.Add(LimitStates.SLS); }
            inputData.CalcTermsList.Clear();
            if (ShortTerm == true) { inputData.CalcTermsList.Add(CalcTerms.ShortTerm); }
            if (LongTerm == true) { inputData.CalcTermsList.Add(CalcTerms.LongTerm); }
        }
    }
}
