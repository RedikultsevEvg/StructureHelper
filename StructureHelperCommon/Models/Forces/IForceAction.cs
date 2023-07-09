using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Action as force load
    /// </summary>
    public interface IForceAction : IAction
    {
        /// <summary>
        /// True means force action is put in center of gravity
        /// </summary>
        bool SetInGravityCenter { get; set; }
        /// <summary>
        /// Point of applying of force load
        /// </summary>
        IPoint2D ForcePoint { get; set; }
        /// <summary>
        /// Return combination of forces
        /// </summary>
        /// <returns></returns>
        IForceCombinationList GetCombinations();
    }
}
