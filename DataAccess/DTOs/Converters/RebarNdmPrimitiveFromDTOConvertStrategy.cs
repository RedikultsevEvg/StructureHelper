using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
