using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.UserControls.States;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    public class ValueDiagramCalculatorInputDataViewModel : ViewModelBase
    {
        private IValueDiagramCalculatorInputData inputData;
        private ICrossSectionRepository repository;

        public bool CheckStrainLimit
        {
            get => inputData.CheckStrainLimit;
            set
            {
                inputData.CheckStrainLimit = value;
                OnPropertyChanged(nameof(CheckStrainLimit));
            }
        }
        public SourceTargetVM<IForceAction> CombinationViewModel { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; }
        public StateCalcTermPairViewModel StateCalcTermPairViewModel { get; }
        public ValueDiagramsViewModel ValueDiagramsViewModel { get; }

        public ValueDiagramCalculatorInputDataViewModel(ICrossSectionRepository repository, IValueDiagramCalculatorInputData inputData)
        {
            this.inputData = inputData;
            this.repository = repository;
            StateCalcTermPairViewModel = new(inputData.StateTermPair);
            ValueDiagramsViewModel = new(inputData.Digrams);
            CombinationViewModel = SourceTargetFactory.GetSourceTargetVM(repository.ForceActions, inputData.ForceActions);
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(repository.Primitives, inputData.Primitives);
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
