using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelperCommon.Infrastructures.Settings
{
    public class CalcTermEntity
    {
        public CalcTerms CalcTerm { get; set; }
        public Color Color { get; set; }
        public string? Name { get; set; }
        public string ShortName { get; set; }
        public CalcTermEntity()
        {
            Color = ColorProcessor.GetRandomColor();
        }
    }
}
