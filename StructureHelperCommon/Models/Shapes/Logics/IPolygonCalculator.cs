using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IPolygonCalculator
    {
        double GetPerimeter(ILinePolygonShape polygon);
        double GetArea(ILinePolygonShape polygon);
        bool ContainsPoint(ILinePolygonShape polygon, IPoint2D point);
    }

}
