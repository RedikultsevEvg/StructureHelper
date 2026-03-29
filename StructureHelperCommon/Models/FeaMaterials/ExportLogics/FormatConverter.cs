using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public static class FormatConverter
    {
        public static string FormatDouble(double value)
        {
            var formatted = Convert.ToString(value, CultureInfo.InvariantCulture);
            return formatted;
        }
    }
}
