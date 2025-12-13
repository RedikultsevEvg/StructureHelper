using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class PrimitiveSet : IPrimitiveSet
    {
        public string Name { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public IEnumerable<IValuePrimitive> ValuePrimitives { get; set;}

        public PrimitiveSet()
        {
            
            ValuePrimitives = new List<IValuePrimitive>();
        }
    }
}
