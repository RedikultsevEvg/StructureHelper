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
    public class LimitStateEntity
    {
        public LimitStates LimitState { get; set; }
        public Color Color { get; set; }
        public string? Name { get; set; }
        public string ShortName { get; set; }
        public LimitStateEntity()
        {
            Color = ColorProcessor.GetRandomColor();
        }
    }
}
