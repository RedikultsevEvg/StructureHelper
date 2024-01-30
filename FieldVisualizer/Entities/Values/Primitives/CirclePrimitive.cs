using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class CirclePrimitive : ICirclePrimitive
    {
        public double Diameter { get; set; }
        public double Value { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Area => Math.PI * Math.Pow(Diameter, 2) / 4;
    }
}
