using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class RectanglePrimitive : IRectanglePrimitive
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Value { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double Area => Height * Width;
    }
}
