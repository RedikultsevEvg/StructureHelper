using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Properties of force action
    /// </summary>
    public interface IForceActionProperty
    {
        /// <summary>
        /// True means force action is put in center of gravity
        /// </summary>
        bool SetInGravityCenter { get; set; }
        /// <summary>
        /// Point of applying of force load
        /// </summary>
        IPoint2D ForcePoint { get; set; }
    }
}
