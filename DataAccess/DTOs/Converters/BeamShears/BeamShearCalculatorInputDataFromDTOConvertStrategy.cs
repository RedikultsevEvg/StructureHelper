using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    internal class BeamShearCalculatorInputDataFromDTOConvertStrategy : ConvertStrategy<BeamShearCalculatorInputData, BeamShearCalculatorInputDataDTO>
    {
        private IUpdateStrategy<IBeamShearCalculatorInputData> updateStrategy;
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;

        public BeamShearCalculatorInputDataFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearCalculatorInputData GetNewItem(BeamShearCalculatorInputDataDTO source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            actionUpdateStrategy.Update(NewItem, source);
            sectionUpdateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorInputDataUpdateStrategy();
            actionUpdateStrategy ??= new HasBeamShearActionsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
        }
    }
}
