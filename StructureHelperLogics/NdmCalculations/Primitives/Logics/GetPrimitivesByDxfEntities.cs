using netDxf;
using netDxf.Entities;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Exports;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class GetPrimitivesByDxfEntities : IObjectConvertStrategy<List<INdmPrimitive>, IEnumerable<EntityObject>>
    {
        private const double metresToMillimeters = 1000.0;
        private List<INdmPrimitive> primitives = [];
        private IGetDxfLayerLogic layerLogic = new GetDxfLayerLogic();


        public List<INdmPrimitive> Convert(IEnumerable<EntityObject> source)
        {
            GetPrimitives(source);
            return primitives;
        }

        private void GetPrimitives(IEnumerable<EntityObject> source)
        {
            primitives.Clear();
            foreach (var dxfEntity in source)
            {
                if (dxfEntity is Polyline2D polyline2d)
                {
                    primitives.Add(GetShapePolygon(polyline2d));
                }
                else if (dxfEntity is Circle circle)
                {
                    primitives.Add(GetCirclePrimitive(circle));
                }
                else
                {
                    throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(dxfEntity));
                }
            }
        }

        private INdmPrimitive GetCirclePrimitive(Circle circle)
        {
            INdmPrimitive primitive = null;
            double radius = circle.Radius / metresToMillimeters;
            double diameter = radius * 2.0;
            if (circle.Layer.Name == layerLogic.GetLayerName(LayerNames.StructuralRebars))
            {
                RebarNdmPrimitive rebar = new (Guid.NewGuid())
                {
                    Name = "Imported rebar",
                    Area = Math.PI * radius * radius
                };
                primitive = rebar;
            }
            else
            {
                EllipseNdmPrimitive ellipse = new(Guid.NewGuid())
                {
                    Name = "Imported circle",
                    Width = diameter,
                    Height = diameter,

                };
                primitive = ellipse;
            }
            primitive.Center = new Point2D(circle.Center.X / metresToMillimeters, circle.Center.Y / metresToMillimeters);

            return primitive;
        }

        private INdmPrimitive GetShapePolygon(Polyline2D polyline2d)
        {
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = $"Imported polygon"
            };
            var convertLogic = new Polyline2DToLinePoligonConvertLogic();
            LinePolygonShape polygonShape = convertLogic.Convert(polyline2d);
            shapeNdmPrimitive.SetShape(polygonShape);
            return shapeNdmPrimitive;
        }
    }
}
