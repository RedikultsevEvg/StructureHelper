using System.Collections.Generic;

namespace StructureHelperCommon.Models.Forces
{
    public interface IForceFactoredList : IForceFactoredCombination
    {
        List<IForceTuple> ForceTuples { get;}
    }
}
