using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Models.NdmPrimitives;
using StructureHelperCommon.Models.Shapes;
using Point = StructureHelper.Infrastructure.UI.DataContexts.Point;
using Rectangle = StructureHelper.Infrastructure.UI.DataContexts.Rectangle;

namespace StructureHelper.Services
{
    public interface IPrimitiveRepository
    {
        void Add(PrimitiveBase primitive);
        void Delete(PrimitiveBase primitive);
        IEnumerable<Point> GetPoints();
        IEnumerable<Rectangle> GetRectangles();
    }
    class PrimitiveRepository : IPrimitiveRepository
    {
        List<Point> points = new List<Point>();
        List<Rectangle> rectangles = new List<Rectangle>();

        public void Add(PrimitiveBase primitive)
        {
            switch (primitive)
            {
                case Point point:
                    points.Add(point);
                    break;
                case Rectangle rectangle:
                    rectangles.Add(rectangle);
                    break;
            }
        }
        public void Delete(PrimitiveBase primitive)
        {
            switch (primitive)
            {
                case Point point:
                    points.Remove(point);
                    break;
                case Rectangle rectangle:
                    rectangles.Remove(rectangle);
                    break;
            }
        }

        public IEnumerable<Point> GetPoints() => points;

        public IEnumerable<Rectangle> GetRectangles() => rectangles;
    }
}
