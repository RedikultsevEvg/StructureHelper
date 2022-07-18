using System.Collections.Generic;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.Materials;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public interface ITriangulationLogic
    {
        ITriangulationLogicOptions Options { get; }
        IEnumerable<INdm> GetNdmCollection(IMaterial material);
        void ValidateOptions(ITriangulationLogicOptions options);
    }
}
