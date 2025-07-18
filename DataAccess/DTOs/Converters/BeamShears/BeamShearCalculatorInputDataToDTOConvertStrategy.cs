using DataAccess.DTOs.Converters.BeamShears;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.BeamShears;

namespace DataAccess.DTOs
{
    public class BeamShearCalculatorInputDataToDTOConvertStrategy : ConvertStrategy<BeamShearCalculatorInputDataDTO, IBeamShearCalculatorInputData>
    {
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
            actionUpdateStrategy.Update(NewItem, source);
            sectionUpdateStrategy.Update(NewItem, source);
            stirrupUpdateStrategy.Update(NewItem, source);
            NewItem.DesignRangeProperty = designRangeConvertStrategy.Convert(source.DesignRangeProperty);
            return NewItem;
        }

        private void InitializeStrategies()
        {
            actionUpdateStrategy ??= new HasBeamShearActionsToDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionsToDTORenameStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupsToDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            designRangeConvertStrategy ??= new BeamShearDesignRangePropertyToDTOConvertStrategy(this);
        }
    }
}
