using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace StructureHelper.Windows.BeamShears
{
    internal class InclinedCrackViewModel : ViewModelBase
    {
        public double StartCoord { get; set; }
        public double EndCoord { get; set; }
        public Color Color { get; set; }
    }
}
