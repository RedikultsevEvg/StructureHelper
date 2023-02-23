using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public interface IValuePrimitive
    {
        double Value { get; }
        double CenterX { get; }
        double CenterY { get; }
        double Area { get; }
    }
}
