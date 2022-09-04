using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class PrimitiveSet : IPrimitiveSet
    {
        public string Name { get; set; }
        public IEnumerable<IValuePrimitive> ValuePrimitives { get; set;}

        public PrimitiveSet()
        {
            Name = "New set of primitives";
            ValuePrimitives = new List<IValuePrimitive>();
        }
    }
}
