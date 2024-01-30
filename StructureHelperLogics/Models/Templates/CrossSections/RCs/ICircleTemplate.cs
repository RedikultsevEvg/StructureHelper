using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public interface ICircleTemplate : IRCSectionTemplate
    {
        ICircleShape Shape { get; }
        double CoverGap { get; set; }
        int BarCount { get; set; }
        double BarDiameter { get; set; }
    }
}
