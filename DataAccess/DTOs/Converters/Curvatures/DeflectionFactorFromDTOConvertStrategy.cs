using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace DataAccess.DTOs
{
    public class DeflectionFactorFromDTOConvertStrategy : ConvertStrategy<DeflectionFactor, DeflectionFactorDTO>
    {
        private IUpdateStrategy<IDeflectionFactor> updateStrategy;
        private IConvertStrategy<ForceTuple, ForceTupleDTO> forceTupleConvertStrategy;

        public DeflectionFactorFromDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }

        private IUpdateStrategy<IDeflectionFactor> UpdateStrategy => updateStrategy ??= new DeflectionFactorUpdateStrategy() { UpdateChildren = false };
        private IConvertStrategy<ForceTuple, ForceTupleDTO> ForceTupleConvertStrategy => forceTupleConvertStrategy ??= new ForceTupleFromDTOConvertStrategy(ReferenceDictionary, TraceLogger);

        public override DeflectionFactor GetNewItem(DeflectionFactorDTO source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            if (source.DeflectionFactors is not ForceTupleDTO deflectionFactor)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.DeflectionFactors) + ": deflection factor");
            }
            NewItem.DeflectionFactors = ForceTupleConvertStrategy.Convert(deflectionFactor);
            if (source.MaxDeflections is not ForceTupleDTO maxDeflections)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(source.MaxDeflections) + ": maximum deflections");
            }
            NewItem.MaxDeflections = ForceTupleConvertStrategy.Convert(maxDeflections);
            return NewItem;
        }
    }
}
