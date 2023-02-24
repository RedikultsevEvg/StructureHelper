using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class CrossSectionViewVisualProperty
    {
        public double AxisLineThickness { get; set; }
        public double GridLineThickness { get; set; }
        public double GridSize { get; set; }
        public double WorkPlainWidth { get; set; }
        public double WorkPlainHeight { get; set; }

        public CrossSectionViewVisualProperty()
        {
            AxisLineThickness = 2d;
            GridLineThickness = 0.25d;
            GridSize = 0.05d;
            WorkPlainWidth = 2.4d;
            WorkPlainHeight = 2.0d;
        }
    }
}
