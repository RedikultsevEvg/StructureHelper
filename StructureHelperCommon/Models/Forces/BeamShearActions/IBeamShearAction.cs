using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IBeamShearAction : IAction
    {
        IBeamShearAxisAction XAxisShearAction { get; }
        IBeamShearAxisAction YAxisShearAction { get; }
    }
}
