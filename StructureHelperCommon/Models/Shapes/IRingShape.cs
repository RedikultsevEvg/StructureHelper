using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IRingShape : IShape
    {
        double OuterDiameter { get; set; }
        double InnerDiameter { get; set; }
        double OuterRadius { get; set; }
        double InnerRadius { get; set; }

    }
}
