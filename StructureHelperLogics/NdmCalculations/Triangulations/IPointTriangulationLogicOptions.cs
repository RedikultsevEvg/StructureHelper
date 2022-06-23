using StructureHelperLogics.Data.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface IPointTriangulationLogicOptions : ITriangulationLogicOptions
    {
        ICenter Center { get; }
        double Area { get; }
    }
}
