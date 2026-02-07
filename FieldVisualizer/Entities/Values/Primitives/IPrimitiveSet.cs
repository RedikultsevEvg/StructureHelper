using System.Collections.Generic;

namespace FieldVisualizer.Entities.Values.Primitives
{
    public interface IPrimitiveSet
    {
        string Name { get; set; }
        string SubTitle { get; set; }
        List<IValuePrimitive> ValuePrimitives { get; }
        List<IShadedPrimitive> ShadedPrimitives { get; }

    }
}
