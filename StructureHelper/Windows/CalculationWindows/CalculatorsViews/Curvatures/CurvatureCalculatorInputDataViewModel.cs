using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    public class CurvatureCalculatorInputDataViewModel : ViewModelBase
    {
        private ICurvatureCalculatorInputData inputData;

        public DeflectionFactorViewModel DeflectionFactor { get; }

        public SourceTargetVM<IForceAction> CombinationViewModel { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; }

        public CurvatureCalculatorInputDataViewModel(ICurvatureCalculatorInputData inputData, ICrossSectionRepository repository)
        {
            this.inputData = inputData;
            CombinationViewModel = SourceTargetFactory.GetSourceTargetVM(repository.ForceActions, inputData.ForceActions);
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(repository.Primitives, inputData.Primitives);
            DeflectionFactor = new(inputData.DeflectionFactor);
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
        }
    }
}
