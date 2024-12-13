using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions
{
    public class GraphPoint : ICloneable
    {
        public bool Visible {  get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public GraphPoint(double x, double y)
        {
            X = x;
            Y = y;
            Visible = true;
        }
        public object Clone()
        {
            var clone = new GraphPoint(X,Y);
            return clone;
        }
    }
}
