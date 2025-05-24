using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    /// <summary>
    /// Implement logic for calculation of bearing capacity of inclined section for shear
    /// </summary>
    public interface IBeamShearStrenghLogic : ILogic
    {
        /// <summary>
        /// Returns Bearing capacity of inclined section for shear
        /// </summary>
        /// <returns>Bearing capacity of inclined section for shear, N</returns>
        double GetShearStrength();
    }
}
