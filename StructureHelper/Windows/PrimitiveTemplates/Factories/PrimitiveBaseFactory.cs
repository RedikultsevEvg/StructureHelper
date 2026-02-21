using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelper.Windows.PrimitiveTemplates.Factories
{
    public class PrimitiveBaseFactory : IPrimitiveBaseFactory
    {
        public PrimitiveBase GetPrimitive(PrimitiveType primitiveType)
        {
            PrimitiveBase viewPrimitive;
            if (primitiveType == PrimitiveType.Rectangle)
            {
                RectangleNdmPrimitive primitive = GetNewRectanglePrimitive();
                viewPrimitive = new RectangleViewPrimitive(primitive);

            }
            else if (primitiveType == PrimitiveType.Reinforcement)
            {
                RebarNdmPrimitive primitive = GetNewReinforcementPrimitive();
                viewPrimitive = new ReinforcementViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Point)
            {
                PointNdmPrimitive primitive = GetNewPointPrimitive();
                viewPrimitive = new PointViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Circle)
            {
                EllipseNdmPrimitive primitive = GetNewCirclePrimitive();
                viewPrimitive = new CircleViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Polygon)
            {
                ShapeNdmPrimitive primitive = GetNewPolygonPrimitive();
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.DoubleTShape)
            {
                ShapeNdmPrimitive primitive = GetNewDoubleTShapePrimitive();
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.TShape)
            {
                ShapeNdmPrimitive primitive = GetNewTShapePrimitive();
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.OShape)
            {
                ShapeNdmPrimitive primitive = GetNewOShapePrimitive();
                viewPrimitive = new RingShapeViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Trapezoid)
            {
                ShapeNdmPrimitive primitive = GetNewTrapezoidPrimitive();
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(primitiveType));
            }
            return viewPrimitive;
        }

        private ShapeNdmPrimitive GetNewTrapezoidPrimitive()
        {
            TrapezoidShape shape = new()
            {
                Height = 0.6,
                TopBase = 0.4,
                BottomBase = 0.2,
            };
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = "New trapezoid primitive"
            };
            shapeNdmPrimitive.SetShape(shape);
            return shapeNdmPrimitive;
        }

        private ShapeNdmPrimitive GetNewDoubleTShapePrimitive()
        {
            VerticalDoubleTShape shape = new()
            {
                FullHeight = 0.6,
                WebThickness = 0.1,
                TopFlangeThickness = 0.1,
                TopFlangeWidth = 0.4,
                BottomFlangeThickness = 0.1,
                BottomFlangeWidth = 0.4,
            };
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = "New I-shape primitive"
            };
            shapeNdmPrimitive.SetShape(shape);
            return shapeNdmPrimitive;
        }

        private ShapeNdmPrimitive GetNewOShapePrimitive()
        {
            RingShape oShape = new()
            {
                OuterDiameter = 0.4,
                InnerDiameter = 0.3
            };
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = "New O-shape primitive"
            };
            shapeNdmPrimitive.SetShape(oShape);
            return shapeNdmPrimitive;
        }

        private ShapeNdmPrimitive GetNewTShapePrimitive()
        {
            VerticalTShape verticalTShape = new()
            {
                FullHeight = 0.6,
                FlangeHeight = 0.1,
                WebWidth = 0.2,
                FlangeWidth = 0.4,
            };
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = "New T-shape primitive"
            };
            shapeNdmPrimitive.SetShape(verticalTShape);
            return shapeNdmPrimitive;
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
