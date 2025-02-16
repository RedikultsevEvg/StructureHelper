using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Implement effective depth for reinforced concrete section
    /// </summary>
    public interface IEffectiveDepth
    {
        /// <summary>
        /// Full depth of cross-section
        /// </summary>
        double FullDepth { get; set; }
        /// <summary>
        /// Effective depth of cross-section
        /// </summary>
        double EffectiveDepth { get; set; }
    }
}
