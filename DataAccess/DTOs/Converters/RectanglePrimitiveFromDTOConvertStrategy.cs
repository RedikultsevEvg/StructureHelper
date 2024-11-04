using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class RectanglePrimitiveFromDTOConvertStrategy : ConvertStrategy<RectangleNdmPrimitive, RectangleNdmPrimitiveDTO>
    {
        private IUpdateStrategy<IRectangleNdmPrimitive> updateStrategy;

        public RectanglePrimitiveFromDTOConvertStrategy(IUpdateStrategy<IRectangleNdmPrimitive> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public RectanglePrimitiveFromDTOConvertStrategy() : this (new RectanglePrimitiveUpdateStrategy())
        {
            
        }

        public override RectangleNdmPrimitive GetNewItem(RectangleNdmPrimitiveDTO source)
        {
            RectangleNdmPrimitive newItem = new(source.Id);
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
