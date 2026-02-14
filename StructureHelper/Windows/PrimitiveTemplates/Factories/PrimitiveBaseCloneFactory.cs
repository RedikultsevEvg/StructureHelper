using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Windows.PrimitiveTemplates.Factories
{
    public class PrimitiveBaseCloneFactory : IPrimitiveBaseCloneFactory
    {
        public PrimitiveBase GetCloneByNdmPrimitive(INdmPrimitive ndmPrimitive)
        {
            var newPrimitive = ndmPrimitive.Clone() as INdmPrimitive;
            newPrimitive.Name += " copy";
            PrimitiveBase primitiveBase;
            if (newPrimitive is IRectangleNdmPrimitive rectangle)
            {
                primitiveBase = new RectangleViewPrimitive(rectangle);
            }
            else if (newPrimitive is IEllipseNdmPrimitive ellipse)
            {
                primitiveBase = new CircleViewPrimitive(ellipse);
            }
            else if (newPrimitive is IShapeNdmPrimitive shapeNDMPrimitive)
            {
                primitiveBase = new ShapeViewPrimitive(shapeNDMPrimitive);
            }
            else if (newPrimitive is IPointNdmPrimitive)
            {
                if (newPrimitive is RebarNdmPrimitive rebar)
                {
                    primitiveBase = new ReinforcementViewPrimitive(rebar);
                }
                else
                {
                    primitiveBase = new PointViewPrimitive(newPrimitive as IPointNdmPrimitive);
                }

            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
            return primitiveBase;
        }
    }
}
