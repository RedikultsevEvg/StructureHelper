using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public interface ICircleBeamTemplate
    {
        ICircleShape Circle { get; }
        double CoverGap { get; set; }
        double SectionDiameter { get; set; }
        int BarQuantity { get; set; }
        double BarDiameter { get; set; }


    }
}
