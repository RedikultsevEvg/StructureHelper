using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.PrimitiveTemplates.Factories
{
    public static class PrimitiveBaseFactory
    {
        public static PrimitiveBase GetPrimitive(PrimitiveType primitiveType)
        {
            PrimitiveBase viewPrimitive;
            INdmPrimitive ndmPrimitive;
            if (primitiveType == PrimitiveType.Rectangle)
            {
                RectangleNdmPrimitive primitive = GetNewRectanglePrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new RectangleViewPrimitive(primitive);

            }
            else if (primitiveType == PrimitiveType.Reinforcement)
            {
                RebarNdmPrimitive primitive = GetNewReinforcementPrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new ReinforcementViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Point)
            {
                PointNdmPrimitive primitive = GetNewPointPrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new PointViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Circle)
            {
                EllipseNdmPrimitive primitive = GetNewCirclePrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new CircleViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Polygon)
            {
                ShapeNdmPrimitive primitive = GetNewPolygonPrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.TShape)
            {
                ShapeNdmPrimitive primitive = GetNewTShapePrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(primitiveType));
            }
            return viewPrimitive;
        }

        private static ShapeNdmPrimitive GetNewTShapePrimitive()
        {
#error
            throw new NotImplementedException();
        }

        public static PrimitiveBase GetCloneByNdmPrimitive(INdmPrimitive ndmPrimitive)
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


        private static ShapeNdmPrimitive GetNewPolygonPrimitive()
        {
            LinePolygonShape polygon = new(Guid.NewGuid());
            polygon.AddVertex(new Vertex(-0.2, 0.3));
            polygon.AddVertex(new Vertex(0.2, 0.3));
            polygon.AddVertex(new Vertex(0.1, 0));
            polygon.AddVertex(new Vertex(0.2, -0.3));
            polygon.AddVertex(new Vertex(-0.2, -0.3));
            polygon.AddVertex(new Vertex(-0.1, 0));
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = "New polygon primitive"
            };
            shapeNdmPrimitive.SetShape(polygon);
            return shapeNdmPrimitive;
        }

        private static EllipseNdmPrimitive GetNewCirclePrimitive()
        {
            return new EllipseNdmPrimitive
            {
                Width = 0.5d
            };
        }

        private static PointNdmPrimitive GetNewPointPrimitive()
        {
            return new PointNdmPrimitive
            {
                Area = 0.0005d
            };
        }

        private static RebarNdmPrimitive GetNewReinforcementPrimitive()
        {
            return new RebarNdmPrimitive
            {
                Area = 0.0005d
            };
        }

        private static RectangleNdmPrimitive GetNewRectanglePrimitive()
        {
            return new RectangleNdmPrimitive
            {
                Width = 0.4d,
                Height = 0.6d
            };
        }
    }
}
