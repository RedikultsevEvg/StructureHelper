using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Implement collection of shear beams load
    /// </summary>
    public interface IHasBeamShearActions
    {
        List<IBeamShearAction> Actions { get; }
    }
}
