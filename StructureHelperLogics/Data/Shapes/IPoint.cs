using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.Shapes
{
    public interface IPoint : IShape
    {
        double Area { get; set; }
    }
}
