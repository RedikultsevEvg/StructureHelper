using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
