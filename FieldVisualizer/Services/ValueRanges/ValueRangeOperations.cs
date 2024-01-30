using FieldVisualizer.Entities.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldVisualizer.Services.ValueRanges
{
    public static class ValueRangeOperations
    {
        public static IEnumerable<IValueRange> DivideValueRange (IValueRange valueRange, int divisionNumber)
        {
            List<IValueRange> valueRanges = new List<IValueRange>();
            if (valueRange.BottomValue == valueRange.TopValue)
            {
                var newRange = new ValueRange() { BottomValue = valueRange.BottomValue, TopValue = valueRange.TopValue };
                valueRanges.Add(newRange);
            }
            else
            {
                double dVal = (valueRange.TopValue - valueRange.BottomValue) / divisionNumber;
                double startBottom = valueRange.BottomValue;
                for (int i = 0; i < divisionNumber; i++ )
                {
                    
                    double currentBottom = startBottom + i * dVal;
                    var newRange = new ValueRange() { BottomValue = currentBottom, TopValue = currentBottom + dVal };
                    valueRanges.Add(newRange);
                }
            }
            return valueRanges;
        }
    }
}
