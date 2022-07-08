using System;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Entities
{
    public interface IMaterialDecorator
    {
        double InitModulus { get; set; }
        IEnumerable<double> DiagramParameters { get; set; }
        Func<IEnumerable<double>, double, double> Diagram { get; set; }
        LoaderCalculator.Data.Materials.IMaterial GetMaterial();
    }
}