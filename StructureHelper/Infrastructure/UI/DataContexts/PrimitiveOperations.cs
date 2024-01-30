using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    internal static class PrimitiveOperations
    {
        
        public static ObservableCollection<PrimitiveBase> ConvertNdmPrimitivesToPrimitiveBase(IEnumerable<INdmPrimitive> primitives)
        {
            ObservableCollection<PrimitiveBase> viewItems = new ObservableCollection<PrimitiveBase>();
            foreach (var primitive in primitives)
            {
                viewItems.Add(ConvertNdmPrimitiveToPrimitiveBase(primitive));
            }
            return viewItems;
        }
        public static PrimitiveBase ConvertNdmPrimitiveToPrimitiveBase(INdmPrimitive primitive)
        {
            PrimitiveBase viewItem;
            if (primitive is IRectanglePrimitive)
            {
                var rect = primitive as IRectanglePrimitive;
                viewItem = new RectangleViewPrimitive(rect);
            }
            else if (primitive is ICirclePrimitive)
            {
                var circle = primitive as ICirclePrimitive;
                viewItem = new CircleViewPrimitive(circle);
            }
            else if (primitive is IPointPrimitive & primitive is not RebarPrimitive)
            {
                var point = primitive as IPointPrimitive;
                viewItem = new PointViewPrimitive(point);
            }
            else if (primitive is RebarPrimitive)
            {
                var point = primitive as RebarPrimitive;
                viewItem = new ReinforcementViewPrimitive(point);
            }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $". Actual type: {primitive.GetType()}");
            return viewItem;
        }
    }
}
