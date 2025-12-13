using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    /// <summary>
    /// Repository of members of cross-section
    /// </summary>
    public interface ICrossSectionRepository : ISaveable, IHasHeadMaterials, IHasForcesAndPrimitives, IHasCalculators
    {
       IRepositoryOperationsLogic Operations { get; }
    }
}
