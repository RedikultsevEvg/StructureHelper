using System;
using System.Collections.Generic;
using System.Text;

namespace FieldVisualizer.Entities.Values
{
    public interface IValueRange
    {
        double TopValue { get; set; }
        double BottomValue { get; set; }
    }
}
