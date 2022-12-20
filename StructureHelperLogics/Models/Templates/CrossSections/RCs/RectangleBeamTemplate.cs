using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.RCs
{
    public class RectangleBeamTemplate : IRectangleBeamTemplate
    {
        public IShape Shape { get; }
        public double CoverGap { get; set; }
        public double TopDiameter { get; set; }
        public double BottomDiameter { get; set; }
        public int WidthCount { get; set; }
        public int HeightCount { get; set; }

        public RectangleBeamTemplate()
        {
            Shape = new RectangleShape() { Width = 0.4d, Height = 0.6d };
            CoverGap = 0.05d;
            TopDiameter = 0.016d;
            BottomDiameter = 0.025d;
            WidthCount = 2;
            HeightCount = 2;
        }

        public RectangleBeamTemplate(double width, double height)
        {
            Shape = new RectangleShape() { Width = width, Height = height };
            CoverGap = 0.05d;
            TopDiameter = 0.016d;
            BottomDiameter = 0.025d;
            WidthCount = 2;
            HeightCount = 2;
        }
    }
}
