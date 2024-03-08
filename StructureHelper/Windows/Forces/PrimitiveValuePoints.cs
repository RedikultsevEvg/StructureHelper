using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Models.Shapes;
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
        public SelectItemsVM<NamedValue<IPoint2D>> ValuePoints { get; set; }

        public PrimitiveValuePoints(PrimitiveBase primitiveBase)
        {
            var ndmPrimitive = primitiveBase.GetNdmPrimitive();
            var pointCollection = ndmPrimitive.GetValuePoints();
            ValuePoints = new SelectItemsVM<NamedValue<IPoint2D>>(pointCollection)
            {
                ShowButtons = false
            };
        }
    }
}
