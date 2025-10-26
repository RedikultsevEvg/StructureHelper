using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class EllipseNdmPrimitiveFromDTOConvertStrategy : ConvertStrategy<EllipseNdmPrimitive, EllipseNdmPrimitiveDTO>
    {
        private IUpdateStrategy<IEllipseNdmPrimitive> updateStrategy = new EllipsePrimitiveUpdateStrategy();
        public override EllipseNdmPrimitive GetNewItem(EllipseNdmPrimitiveDTO source)
        {
            EllipseNdmPrimitive newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
