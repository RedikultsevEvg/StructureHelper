using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface IProcessTupleInputData : IInputData
    {
        List<INdm> Ndms { get; set; }
        IForceCombinationList Combination { get; set; }
        IDesignForceTuple Tuple { get; set; }
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
    }
}
