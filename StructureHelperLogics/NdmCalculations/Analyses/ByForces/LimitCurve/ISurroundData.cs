using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public interface ISurroundData : ICloneable
    {
        double ConstZ { get; set; }
        ConstOneDirectionConverter ConvertLogicEntity { get; set; }
        double XMax { get; set; }
        double XMin { get; set; }
        double YMax { get; set; }
        double YMin { get; set; }
    }
}