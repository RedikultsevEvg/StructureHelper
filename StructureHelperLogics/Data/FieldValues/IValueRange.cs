using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.FieldValues
{
    public interface IValueRange
    {
        double TopValue { get; set; }
        double BottomValue { get; set; }
    }
}
