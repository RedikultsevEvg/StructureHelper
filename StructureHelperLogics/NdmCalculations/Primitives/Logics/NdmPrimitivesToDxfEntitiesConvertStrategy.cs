using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Exports;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class NdmPrimitivesToDxfEntitiesConvertStrategy : IObjectConvertStrategy<List<(EntityObject, LayerNames)>, IEnumerable<INdmPrimitive>>
    {
        private const double metresToMillimeters = 1000.0;
        private List<(EntityObject, LayerNames)> entities = [];
        private IShapeConvertStrategy<EntityObject, IShape> shapeConvertStrategy = new ShapeToDxfEntityConvertStrategy() { Scale = metresToMillimeters };


        public List<(EntityObject, LayerNames)> Convert(IEnumerable<INdmPrimitive> source)
        {
            entities.Clear();
            foreach (var ndmPrimitive in source)
            {
                shapeConvertStrategy.Dx = ndmPrimitive.Center.X;
                shapeConvertStrategy.Dy = ndmPrimitive.Center.Y;
                if (ndmPrimitive is IRectangleNdmPrimitive rectangleNdmPrimitive)
                {
                    var rectangle = shapeConvertStrategy.Convert(rectangleNdmPrimitive);
                    entities.Add((rectangle, LayerNames.StructiralPrimitives));
                }
                else if (ndmPrimitive is IShapeNdmPrimitive shapeNdmPrimitive)
                {
                    var polygon = shapeConvertStrategy.Convert(shapeNdmPrimitive.Shape);
                    entities.Add((polygon, LayerNames.StructiralPrimitives));
                }
                else if (ndmPrimitive is IEllipseNdmPrimitive ellipseNdmPrimitive)
                {
                    CircleShape circleShape = new CircleShape() { Diameter = ellipseNdmPrimitive.Width};
                    var circle = shapeConvertStrategy.Convert(circleShape);
                    entities.Add((circle, LayerNames.StructiralPrimitives));
                }
                else if (ndmPrimitive is IRebarNdmPrimitive rebar)
                {
                    LayerNames layerName = LayerNames.StructuralRebars;
                    convertPoint(rebar, layerName);
                }
                else if (ndmPrimitive is IPointNdmPrimitive point)
                {
                    LayerNames layerName = LayerNames.StructiralPrimitives;
                    convertPoint(point, layerName);
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(ndmPrimitive));
                }
            }
            return entities;
        }

        private void convertPoint(IPointNdmPrimitive point, LayerNames layerName)
        {
            double diameter = Math.Sqrt(point.Area / Math.PI * 4.0);
            CircleShape circleShape = new CircleShape() { Diameter = diameter };
            var circle = shapeConvertStrategy.Convert(circleShape);
            entities.Add((circle, layerName));
        }
    }
}
