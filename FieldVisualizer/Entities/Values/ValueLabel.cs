using System;
using System.Collections.Generic;
using System.Text;

namespace FieldVisualizer.Entities.Values
{
    public class ValueLabel : IValueLabel
    {
        public bool IsActive { get; set; } = true;
        public string Value { get; set; } = string.Empty;
        public double X { get; set; }
        public double Y { get; set; }
    }
}
