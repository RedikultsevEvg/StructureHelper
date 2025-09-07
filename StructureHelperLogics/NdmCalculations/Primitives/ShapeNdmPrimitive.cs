using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class ShapeNdmPrimitive : IShapeNDMPrimitive
    {
        private IShape shape;

        public Guid Id { get; }
        public string? Name { get; set; } = string.Empty;
        public IPoint2D Center { get; set; } = new Point2D();
        public IShape Shape => shape;
        public INdmElement NdmElement { get; } = new NdmElement(Guid.NewGuid());
        public ICrossSection? CrossSection { get; set; }
        public IVisualProperty VisualProperty { get; } = new VisualProperty { Opacity = 0.8d };
        public double RotationAngle { get; set; }
        public IDivisionSize DivisionSize { get; } = new DivisionSize(Guid.NewGuid());
        public ShapeNdmPrimitive(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            throw new NotImplementedException();
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            var points = new List<INamedAreaPoint>();
            INamedAreaPoint newPoint;
            newPoint = new NamedAreaPoint
            {
                Name = "Center",
                Point = Center.Clone() as Point2D,
                Area = 0d
            };
            points.Add(newPoint);
            if (shape is IPolygonShape polygon)
            {
                int i = 0;
                foreach (var item in polygon.Vertices)
                {
                    newPoint = new NamedAreaPoint
                    {
                        Name = $"Vertex {i}",
                        Point = item.Point.Clone() as IPoint2D,
                        Area = 0d
                    };
                    points.Add(newPoint);
                    i++;
                }
            }
            return points;
        }

        public bool IsPointInside(IPoint2D point)
        {
            throw new NotImplementedException();
        }

        public void SetShape(IShape shape)
        {
            this.shape = shape;
        }
    }
}
