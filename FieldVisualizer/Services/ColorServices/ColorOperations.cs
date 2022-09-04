using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace FieldVisualizer.Services.ColorServices
{
    public static class ColorOperations
    {
        const byte Alpha = 0xff;
        public static Color GetColorByValue(IValueRange range, IColorMap map, double val)
        {
            double minVal = range.BottomValue;
            double maxVal = range.TopValue;
            if (range.TopValue == minVal || map.Colors.Count == 0) { return map.Colors[0]; }
            if (val > maxVal || val < minVal) { return Colors.Gray; }
            if (val == minVal) { return map.Colors[0]; }
            if (val == maxVal) { return map.Colors[map.Colors.Count - 1]; }

            double valPerc = (val - minVal) / (maxVal - minVal);// value%
            double colorPerc = 1d / (map.Colors.Count - 1); // % of each block of color. the last is the "100% Color"
            double blockOfColor = valPerc / colorPerc;// the integer part repersents how many block to skip
            int blockIdx = (int)Math.Truncate(blockOfColor);// Idx of 
            double valPercResidual = valPerc - (blockIdx * colorPerc);//remove the part represented of block 
            double percOfColor = valPercResidual / colorPerc;// % of color of this block that will be filled

            Color cTarget = map.Colors[blockIdx];
            Color cNext = map.Colors[blockIdx + 1];

            var deltaR = cNext.R - cTarget.R;
            var deltaG = cNext.G - cTarget.G;
            var deltaB = cNext.B - cTarget.B;

            var R = cTarget.R + (deltaR * percOfColor);
            var G = cTarget.G + (deltaG * percOfColor);
            var B = cTarget.B + (deltaB * percOfColor);

            Color c = map.Colors[0];
            c = Color.FromArgb(Alpha, (byte)R, (byte)G, (byte)B);
            return c;
        }
        public static IEnumerable<IValueColorRange> GetValueColorRanges(IValueRange fullRange, IEnumerable<IValueRange> valueRanges, IColorMap colorMap)
        {
            var colorRanges = new List<IValueColorRange>(); 
            foreach (var valueRange in valueRanges)
            {
                IValueColorRange valueColorRange = new ValueColorRange();
                valueColorRange.IsActive = true;
                valueColorRange.BottomValue = valueRange.BottomValue;
                valueColorRange.AverageValue = (valueRange.BottomValue + valueRange.TopValue) / 2;
                valueColorRange.TopValue = valueRange.TopValue;
                valueColorRange.BottomColor = GetColorByValue(fullRange, colorMap, valueColorRange.BottomValue);
                valueColorRange.TopColor = GetColorByValue(fullRange, colorMap, valueColorRange.TopValue);
                colorRanges.Add(valueColorRange);
            }
            return colorRanges;
        }
    }
}
