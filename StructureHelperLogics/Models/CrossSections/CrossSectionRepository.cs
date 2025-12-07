using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepository : ICrossSectionRepository
    {
        public Guid Id { get; }
        public List<IForceAction> ForceActions { get; private set; } = new();
        public List<IHeadMaterial> HeadMaterials { get; private set; } = new();
        public List<INdmPrimitive> Primitives { get; } = new();
        public List<ICalculator> Calculators { get; private set; } = new();

        public CrossSectionRepository(Guid id)
        {
            Id = id;
        }

        public CrossSectionRepository() : this(Guid.NewGuid())
        {
        }
    }
}
