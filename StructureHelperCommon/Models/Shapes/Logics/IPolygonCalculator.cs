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
        double GetPerimeter(IPolygonShape polygon);
        double GetArea(IPolygonShape polygon);
        bool ContainsPoint(IPolygonShape polygon, IPoint2D point);
    }

}
