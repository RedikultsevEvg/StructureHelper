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
        double SupportShearForce { get; set; }
        List<IBeamShearLoad> ShearLoads {get;}
    }
}
