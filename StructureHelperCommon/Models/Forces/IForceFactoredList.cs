using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceFactoredList : IForceFactoredCombination, ICloneStrategy<IForceFactoredList>
    {
        List<IForceTuple> ForceTuples { get;}
    }
}
