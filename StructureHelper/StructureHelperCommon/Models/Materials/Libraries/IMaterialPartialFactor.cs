using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IMaterialPartialFactor : IPartialFactor
    {
        StressStates StressState { get; set; }
        CalcTerms CalcTerm { get; set; }
        LimitStates LimitState { get; set; }
    }
}
