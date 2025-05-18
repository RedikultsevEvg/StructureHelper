using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implement properties for shear beam load 
    /// </summary>
    public interface IBeamSpanLoad : IAction
    {
        /// <summary>
        /// Value of level where action is applyied at, 0.5 is top surface of beam, -0.5 is bottom surface of beam
        /// </summary>
        double RelativeLoadLevel { get; set; }
        /// <summary>
        /// Ratio of substraction load from total shear force 
        /// </summary>
        double LoadRatio { get; set; }
        /// <summary>
        /// Properties of combination of forces
        /// </summary>
        IFactoredCombinationProperty CombinationProperty { get; set; }
    }
}
