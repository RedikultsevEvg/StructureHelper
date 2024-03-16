using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class PrimitiveValuePoints
    {
        public PrimitiveBase PrimitiveBase {get;set;}
        public SelectItemsVM<INamedAreaPoint> ValuePoints { get; set; }

        public PrimitiveValuePoints(PrimitiveBase primitiveBase)
        {
            var ndmPrimitive = primitiveBase.GetNdmPrimitive();
            var pointCollection = ndmPrimitive.GetValuePoints();
            ValuePoints = new SelectItemsVM<INamedAreaPoint>(pointCollection)
            {
                ShowButtons = false
            };
        }
    }
}
