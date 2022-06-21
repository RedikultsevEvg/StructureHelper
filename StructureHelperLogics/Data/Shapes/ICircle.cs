using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.Shapes
{
    public interface ICircle : IShape
    {
        double Diameter { get; set; }
    }
}
