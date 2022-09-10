using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    /// <summary>
    /// Represent circle primitive
    /// </summary>
    public interface ICirclePrimitive : IValuePrimitive
    {
        double Diameter { get; set; }
    }
}
