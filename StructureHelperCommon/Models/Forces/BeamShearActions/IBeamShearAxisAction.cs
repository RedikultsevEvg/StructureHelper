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
        /// Shear force at support point, N
        /// </summary>
        double SupportShearForce { get; set; }
        /// <summary>
        /// Properties of combination of forces
        /// </summary>
        IFactoredCombinationProperty FactoredCombinationProperty { get; }
        /// <summary>
        /// Collection of loads which are applyed on beam at its span
        /// </summary>
        List<IBeamShearLoad> ShearLoads {get;}
    }
}
