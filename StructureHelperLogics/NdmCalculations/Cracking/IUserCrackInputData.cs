using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public interface IUserCrackInputData : IInputData, ISaveable
    {
        double LengthBetweenCracks { get; set; }
        bool SetLengthBetweenCracks { get; set; }
        bool SetSofteningFactor { get; set; }
        double SofteningFactor { get; set; }
        double UltimateLongCrackWidth { get; set; }
        double UltimateShortCrackWidth { get; set; }
    }
}