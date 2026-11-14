using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceFactoredList : IForceFactoredCombination, ISelfCloneStrategy<IForceFactoredList>
    {
        List<IForceTuple> ForceTuples { get;}
    }
}
