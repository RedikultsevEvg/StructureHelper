using System.Collections.Generic;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Materials;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationLogic
    {
        IEnumerable<INdm> GetNdmCollection();
        void ValidateOptions(ITriangulationLogicOptions options);
    }
}
