using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections.RCs
{
    public class CircleTemplate : ICircleTemplate
    {
        public ICircleShape Shape { get; }
        public double CoverGap { get; set; }
        public int BarCount { get; set; }
        public double BarDiameter { get; set; }
        public CircleTemplate()
        {
            Shape = new CircleShape();
            Shape.Diameter = 0.5d;
            CoverGap = 0.05d;
            BarCount = 8;
            BarDiameter = 0.025d;
        }
    }
}
