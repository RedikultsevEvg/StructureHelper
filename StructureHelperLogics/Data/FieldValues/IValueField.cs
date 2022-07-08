using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.FieldValues
{
    public interface IValueField
    {
        IValueRange ValueRange { get; set; }
        IEnumerable<IValueShape> ValueShapes { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
