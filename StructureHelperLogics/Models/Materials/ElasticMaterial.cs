using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Services;

namespace StructureHelperLogics.Models.Materials
{
    public class ElasticMaterial : IElasticMaterial
    {
        private IElasticMaterialLogic elasticMaterialLogic => new ElasticMaterialLogic();
        public double Modulus { get; set; }
        public double CompressiveStrength { get; set; }
        public double TensileStrength { get; set; }
        public List<IMaterialSafetyFactor> SafetyFactors { get; set; } = new();

        public Guid Id { get; }

        public ElasticMaterial(Guid id)
        {
            Id = id;
        }

        public ElasticMaterial() : this(Guid.NewGuid())
        {
            
        }

        public IMaterial GetLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            var material = elasticMaterialLogic.GetLoaderMaterial(this, limitState, calcTerm);
            return material;
        }

        public IMaterial GetCrackedLoaderMaterial(LimitStates limitState, CalcTerms calcTerm)
        {
            return GetLoaderMaterial(limitState, calcTerm);
        }

        public IHelperMaterial GetClone(IHelperMaterial sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }

        public IHelperMaterial GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ElasticMaterial newItem = new(id);
            var updateStrategy = new ElasticUpdateStrategy();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
