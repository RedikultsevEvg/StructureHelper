using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.BeamShears;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearCalculatorInputDataViewModel : ViewModelBase
    {
        private readonly IBeamShearRepository shearRepository;
        private readonly IBeamShearCalculatorInputData inputData;

        public SourceTargetVM<IBeamShearAction> ActionSourceTarget { get; } = new();
        public SourceTargetVM<IStirrup> StirrupSourceTarget { get; } = new();
        public SourceTargetVM<IBeamShearSection> SectionSourceTarget { get; } = new();

        public BeamShearCalculatorInputDataViewModel(IBeamShearRepository shearRepository, IBeamShearCalculatorInputData inputData)
        {
            this.shearRepository = shearRepository;
            this.inputData = inputData;
            InitializeViewModels();
        }

        private void InitializeViewModels()
        {
            ActionSourceTarget.SetTargetItems(inputData.Actions);
            ActionSourceTarget.SetSourceItems(shearRepository.Actions);
            ActionSourceTarget.ItemDataDemplate = SourceTargetFactory.GetSimpleTemplate();
            StirrupSourceTarget.SetTargetItems(inputData.Stirrups);
            StirrupSourceTarget.SetSourceItems(shearRepository.Stirrups);
            StirrupSourceTarget.ItemDataDemplate = SourceTargetFactory.GetSimpleTemplate();
            SectionSourceTarget.SetTargetItems(inputData.Sections);
            SectionSourceTarget.SetSourceItems(shearRepository.Sections);
            SectionSourceTarget.ItemDataDemplate = SourceTargetFactory.GetSimpleTemplate();
        }
    }
}
