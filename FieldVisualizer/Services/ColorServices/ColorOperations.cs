using FieldVisualizer.Entities.ColorMaps;
using FieldVisualizer.Entities.Values;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace FieldVisualizer.Services.ColorServices
{
    public static class ColorOperations
    {
        const byte Alpha = 0xff;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullRange"></param>
        /// <param name="valueRanges"></param>
        /// <param name="colorMap"></param>
        /// <returns></returns>
        public static IEnumerable<IValueColorRange> GetValueColorRanges(IValueRange fullRange, IEnumerable<IValueRange> valueRanges, IColorMap colorMap)
        {
            var colorRanges = new List<IValueColorRange>(); 
            foreach (var valueRange in valueRanges)
            {
                IValueColorRange valueColorRange = new ValueColorRange
                {
                    IsActive = true,
                    BottomValue = valueRange.BottomValue,
                    AverageValue = (valueRange.BottomValue + valueRange.TopValue) / 2,
                    TopValue = valueRange.TopValue
                };
                valueColorRange.BottomColor = GetColorByValue(fullRange, colorMap, valueColorRange.BottomValue);
                valueColorRange.TopColor = GetColorByValue(fullRange, colorMap, valueColorRange.TopValue);
                colorRanges.Add(valueColorRange);
            }
            return colorRanges;
        }
        /// <summary>
        /// Returns color by value, range of value an color map
        /// </summary>
        /// <param name="range">Range of valoue</param>
        /// <param name="map">Color map</param>
        /// <param name="val">Value</param>
        /// <returns></returns>
        public static Color GetColorByValue(IValueRange range, IColorMap map, double val)
        {
            CheckColorMap(map);
            if (range.TopValue == range.BottomValue || map.Colors.Count == 1) //if range width is zero or map contain just 1 color
            {
                return map.Colors[0];
            }
            var valueRange = GetExtendedRange(range);
            if (val >= valueRange.TopValue || val <= valueRange.BottomValue)
            {
                return GetColorValueIsOutOfRange(valueRange, map, val);
            }
            return GetColorValueIsInRange(valueRange, map, val);
        }

        private static Color GetColorValueIsOutOfRange(IValueRange range, IColorMap map, double val)
        {
            if (val > range.TopValue || val < range.BottomValue)
            {
                return Colors.Gray;
            }
            if (val == range.BottomValue)
            {
                return map.Colors[0];
            }
            if (val == range.TopValue)
            {
                return map.Colors[^1];
            }
            throw new StructureHelperException(ErrorStrings.DataIsInCorrect);
        }

        private static Color GetColorValueIsInRange(IValueRange range, IColorMap map, double val)
        {
            var deltaVal = val - range.BottomValue;
            var rangeWidth = range.TopValue - range.BottomValue;
            var valPerc = deltaVal / rangeWidth; // percent of value on the distance from minValue to maxValue
            if (valPerc >= 1d)
            {
                return map.Colors[^1];
            }
            double colorPerc = 1d / (map.Colors.Count - 1d); // % of each block of color. the last is the "100% Color"
            double blockOfColor = valPerc / colorPerc;// the integer part represents how many block to skip
            int blockIdx = (int)Math.Truncate(blockOfColor);// Idx of
            double valPercResidual = valPerc - (blockIdx * colorPerc);//remove the part represented of block 
            double percOfColor = valPercResidual / colorPerc;// % of color of this block that will be filled

            //in some cases due to accuracy of double type percent of color may be less than zero
            if (percOfColor <= 0d)
            {
                return map.Colors[blockIdx];
            }
            Color c = GetColorByColorMap(map, blockIdx, percOfColor);
            return c;
        }

        private static IValueRange GetExtendedRange(IValueRange range)
        {
            var minVal = range.BottomValue - 1e-15d * Math.Abs(range.BottomValue);
            var maxVal = range.TopValue + 1e-15d * Math.Abs(range.TopValue);
            return new ValueRange()
            {
                BottomValue = minVal,
                TopValue = maxVal
            };
        }

        private static Color GetColorByColorMap(IColorMap map, int blockIdx, double percOfColor)
        {
            Color cTarget = map.Colors[blockIdx];
            Color cNext = map.Colors[blockIdx + 1];

            var deltaRed = cNext.R - cTarget.R;
            var deltaGreen = cNext.G - cTarget.G;
            var deltaBlue = cNext.B - cTarget.B;

            var Red = cTarget.R + (deltaRed * percOfColor);
            var Green = cTarget.G + (deltaGreen * percOfColor);
            var Blue = cTarget.B + (deltaBlue * percOfColor);

            Color c = Color.FromArgb(Alpha, (byte)Red, (byte)Green, (byte)Blue);
            return c;
        }

        private static void CheckColorMap(IColorMap map)
        {
            if (map.Colors.Count == 0)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Color map is empty");
            }
        }
    }
}
