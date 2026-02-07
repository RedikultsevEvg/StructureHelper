using System.Collections.Generic;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public class PrimitiveSet : IPrimitiveSet
    {
        public string Name { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public List<IValuePrimitive> ValuePrimitives { get; set; } = new();
        public List<IShadedPrimitive> ShadedPrimitives { get; set; } = new();
    }
}
