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
    public interface ISumForceByShearLoadLogic : ILogic
    {
        /// <summary>
        /// Returns summary force of action from start to end
        /// </summary>
        /// <param name="beamShearLoad">Source action</param>
        /// <param name="startCoord">Coordinate of start point, m</param>
        /// <param name="endCoord">Coordinate of end point, m</param>
        /// <returns>Summary force, N</returns>
        IForceTuple GetSumShearForce(IBeamSpanLoad beamShearLoad, double startCoord, double endCoord);
    }
}
