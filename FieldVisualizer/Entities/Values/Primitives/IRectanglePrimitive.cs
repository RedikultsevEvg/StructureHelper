using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    internal interface IRectanglePrimitive : IValuePrimitive
    {
        double Height { get; set; }
        double Width { get; set; }
    }
}
