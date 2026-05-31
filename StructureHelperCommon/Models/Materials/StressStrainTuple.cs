using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.Materials
{
    public class StressStrainTuple : IStressStrainTuple
    {
        public double Stress { get; set; }
        public double Strain { get; set; }
    }
}
