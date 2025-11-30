using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public interface ITrianglePrimitive : IValuePrimitive
    {
        IPoint2D Point1 { get; set; }
        IPoint2D Point2 { get; set; }
        IPoint2D Point3 { get; set; }
        double ValuePoint1 { get; set; }
        double ValuePoint2 { get; set; }
        double ValuePoint3 { get; set; }
    }
}
