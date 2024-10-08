using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public interface ICrossSectionRepository : ISaveable, IHasHeadMaterials, IHasPrimitives, IHasForceCombinations, IHasCalculators
    {
       
    }
}
