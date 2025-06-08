using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class PointNdmPrimitiveFromDTOConvertStrategy : ConvertStrategy<PointNdmPrimitive, PointNdmPrimitiveDTO>
    {
        private IUpdateStrategy<IPointNdmPrimitive> updateStrategy = new PointNdmPrimitiveUpdateStrategy();


        public override PointNdmPrimitive GetNewItem(PointNdmPrimitiveDTO source)
        {
            PointNdmPrimitive newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
