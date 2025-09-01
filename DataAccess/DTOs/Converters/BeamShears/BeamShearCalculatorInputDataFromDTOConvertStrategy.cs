using DataAccess.DTOs.Converters.BeamShears;
using StructureHelperCommon.Infrastructures.Exceptions;
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
        private IConvertStrategy<BeamShearDesignRangeProperty, BeamShearDesignRangePropertyDTO> designRangeConvertStrategy;

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
            if (source.DesignRangeProperty is BeamShearDesignRangePropertyDTO propertyDTO)
            {
                NewItem.DesignRangeProperty = designRangeConvertStrategy.Convert(propertyDTO);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.DesignRangeProperty));
            }
            return NewItem;
        }

        private void InitializeStrategies()
        {
            updateStrategy ??= new BeamShearCalculatorInputDataUpdateStrategy();
            actionUpdateStrategy ??= new HasBeamShearActionsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            sectionUpdateStrategy ??= new HasBeamShearSectionsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            stirrupUpdateStrategy ??= new HasStirrupsFromDTOUpdateStrategy(ReferenceDictionary, TraceLogger);
            designRangeConvertStrategy ??= new BeamShearDesignRangePropertyFromDTOConvertStrategy(this);
        }
    }
}
