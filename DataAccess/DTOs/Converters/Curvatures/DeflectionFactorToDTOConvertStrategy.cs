using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;

namespace DataAccess.DTOs
{
    public class DeflectionFactorToDTOConvertStrategy : ConvertStrategy<DeflectionFactorDTO, IDeflectionFactor>
    {
        private IUpdateStrategy<IDeflectionFactor> updateStrategy;
        private IConvertStrategy<ForceTupleDTO, IForceTuple> forceTupleConvertStrategy;
        private IUpdateStrategy<IDeflectionFactor> UpdateStrategy => updateStrategy??= new DeflectionFactorUpdateStrategy() { UpdateChildren = false};
        private IConvertStrategy<ForceTupleDTO, IForceTuple> ForceTupleConvertStrategy => forceTupleConvertStrategy ??= new ForceTupleToDTOConvertStrategy(ReferenceDictionary, TraceLogger);

        public DeflectionFactorToDTOConvertStrategy(IBaseConvertStrategy baseConvertStrategy) : base(baseConvertStrategy)
        {
        }
        public override DeflectionFactorDTO GetNewItem(IDeflectionFactor source)
        {
            NewItem = new(source.Id);
            UpdateStrategy.Update(NewItem, source);
            NewItem.DeflectionFactors = ForceTupleConvertStrategy.Convert(source.DeflectionFactors);
            NewItem.MaxDeflections = ForceTupleConvertStrategy.Convert(source.MaxDeflections);
            return NewItem;
        }
    }
}
