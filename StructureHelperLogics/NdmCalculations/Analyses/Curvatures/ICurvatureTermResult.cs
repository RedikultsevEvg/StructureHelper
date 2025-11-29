using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureTermResult : IResult
    {
        IDeflectionResult DeflectionMx { get; set; }
        IDeflectionResult DeflectionMy { get; set; }
        IDeflectionResult DeflectionNz { get; set; }

    }
}
