using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
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
            foreach (var item in primitives)
            {
                if (item is IRectanglePrimitive)
                {
                    var rect = item as IRectanglePrimitive;
                    viewItems.Add(new RectangleViewPrimitive(rect));
                }
                else if (item is ICirclePrimitive)
                {
                    var circle = item as ICirclePrimitive;
                    viewItems.Add(new CircleViewPrimitive(circle));
                }
                else if (item is IPointPrimitive)
                {
                    var point = item as IPointPrimitive;
                    viewItems.Add(new PointViewPrimitive(point));
                }
                else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
            return viewItems;
        }
    }
}
