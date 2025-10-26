using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class RebarNdmPrimitiveFromDTOConvertStrategy : ConvertStrategy<RebarNdmPrimitive, RebarNdmPrimitiveDTO>
    {
        private IUpdateStrategy<IRebarNdmPrimitive> updateStrategy = new RebarNdmPrimitiveUpdateStrategy();

        public override RebarNdmPrimitive GetNewItem(RebarNdmPrimitiveDTO source)
        {
            RebarNdmPrimitive newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
