using System;
using System.Collections.Generic;
using System.Text;

namespace FieldVisualizer.Entities.Values
{
    public interface IValueLabel
    {
        bool IsActive { get; set; }
        string Value { get; set; }
        double X { get; set; }
        double Y { get; set; }
    }
}
