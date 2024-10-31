using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Services
{
    public static class MathUtils
    {
        public static double Interpolation(double x, double x1, double x2, double y1, double y2)
        {
            if (Math.Abs(x2 - x1) < 10E-3)
            {
                return (y1 + y2) / 2;
            }
            var y = (y2 - y1) * (x - x1) / (x2 - x1) + y1;
            return y;
        }
    }
}
