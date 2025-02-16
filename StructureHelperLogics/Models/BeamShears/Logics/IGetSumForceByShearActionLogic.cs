using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement logic for obtaining of summary force of action from start to end
    /// </summary>
    public interface IGetSumForceByShearActionLogic : ILogic
    {
        /// <summary>
        /// Returns summary force of action from start to end
        /// </summary>
        /// <param name="beamShearAction">Source action</param>
        /// <param name="startCoord">Coordinate of start point, m</param>
        /// <param name="endCoord">Coordinate of end point, m</param>
        /// <returns>Summary force, N</returns>
        double GetSumShearForce(IBeamShearLoad beamShearAction, double startCoord, double endCoord);
    }
}
