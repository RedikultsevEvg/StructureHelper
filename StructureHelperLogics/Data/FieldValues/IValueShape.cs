using StructureHelperLogics.Data.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.FieldValues
{
    public interface IValueShape
    {
        IShape Shape { get; }
        ICenter Center { get; }
        double Value { get; set; }
        string Description { get; set; }
    }
}
