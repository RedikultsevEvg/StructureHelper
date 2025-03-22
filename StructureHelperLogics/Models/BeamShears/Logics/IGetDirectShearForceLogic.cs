using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implements logic of calculating shear force without considering term and limit state
    /// </summary>
    public interface IGetDirectShearForceLogic : ILogic
    {
        /// <summary>
        /// Returns value of shear force at the end of inclined section
        /// </summary>
        /// <returns>Value of shear force at the end of inclined section</returns>
        double CalculateShearForce();
    }
}
