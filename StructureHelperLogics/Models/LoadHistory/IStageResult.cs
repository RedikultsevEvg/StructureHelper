using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Models.LoadHistory
{
    public interface IStageResult : IResult
    {
        IConstructionStage Stage { get; }
        List<ISubStageResult> SubStagesResults { get; }
        Dictionary<INdmPrimitive, INdmPrimitive> Primitives { get; }
    }
}
