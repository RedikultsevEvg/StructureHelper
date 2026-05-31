using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public interface IStressStrainTuple
    {
        double Stress { get; set; }
        double Strain { get; set; }
    }
}
