using System;
using System.Collections.Generic;
using System.Text;

namespace FieldVisualizer.Entities.Values
{
    public class ValueRange : IValueRange
    {
        public double TopValue { get; set; }
        public double BottomValue { get; set; }
    }
}
