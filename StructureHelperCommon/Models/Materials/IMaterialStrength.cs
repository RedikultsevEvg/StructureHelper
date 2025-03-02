using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials
{
    public interface IMaterialStrength
    {
        (double Compressive, double Tensile) GetStrength(LimitStates limitState, CalcTerms calcTerm);
    }
}