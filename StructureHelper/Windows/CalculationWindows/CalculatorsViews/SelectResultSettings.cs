using StructureHelperCommon.Models.Calculators;
using System.Collections.Generic;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class SelectResultSettings
    {
        public string Filename { get; set; } = string.Empty;
        public bool ExportValidResults { get; set; }
        public bool ExportInValidResults { get; set; }
        public IEnumerable<IResult>? Results { get; set; }
    }
}
