using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Shapes
{
    public class Point2DRange : IPoint2DRange
    {
        public Guid Id { get; }
        public IPoint2D StartPoint { get; set; } = new Point2D();
        public IPoint2D EndPoint { get; set; } = new Point2D();

        public Point2DRange(Guid id)
        {
            Id = id;
        }
    }
}
