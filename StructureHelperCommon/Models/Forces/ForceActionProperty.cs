using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <inheritdoc/>
    public class ForceActionProperty : IForceActionProperty
    {
        /// <inheritdoc/>
        public bool SetInGravityCenter { get; set; }
        /// <inheritdoc/>
        public IPoint2D ForcePoint { get; set; }
    }
}
