using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Implement properties of shear loads on beam
    /// </summary>
    public interface IBeamShearAxisAction : IAction
    {
        /// <summary>
        /// Internal force at support point
        /// </summary>
        IFactoredForceTuple SupportForce { get; set; }
        /// <summary>
        /// Collection of loads which are applyed on beam at its span
        /// </summary>
        List<IBeamSpanLoad> ShearLoads {get;}
    }
}
