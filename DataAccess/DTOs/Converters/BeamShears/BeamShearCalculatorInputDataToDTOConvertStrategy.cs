using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<BeamShearCalculatorInputDataDTO, IBeamShearCalculatorInputData>
    {
        private IUpdateStrategy<IBeamShearCalculatorInputData> updateStrategy;
        private IUpdateStrategy<IHasBeamShearActions> actionUpdateStrategy;
        private IUpdateStrategy<IHasBeamShearSections> sectionUpdateStrategy;
        private IUpdateStrategy<IHasStirrups> stirrupUpdateStrategy;
        private IConvertStrategy<BeamShearDesignRangePropertyDTO, IBeamShearDesignRangeProperty> designRangeConvertStrategy;

        public BeamShearCalculatorInputDataToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        public override BeamShearCalculatorInputDataDTO GetNewItem(IBeamShearCalculatorInputData source)
        {
            InitializeStrategies();
            NewItem = new(source.Id);
            updateStrategy.Update(NewItem, source);
            actionUpdateStrategy.Update(NewItem, source);
            sectionUpdateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            NewItem.DesignRangeProperty = designRangeConvertStrategy.Convert(source.DesignRangeProperty);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorInputDataUpdateStrategy();
            actionUpdateStrategy ??= new HasBeamShearActionsToDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionsToDTORenameStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupsToDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            designRangeConvertStrategy ??= new BeamShearDesignRangePropertyToDTOConvertStrategy(this);
        }
    }
}
