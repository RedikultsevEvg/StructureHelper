using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs
{
    public class CrossSectionRepositoryDTO : ICrossSectionRepository
    {
        private IRepositoryOperationsLogic operations;

        [JsonProperty("Id")]        
        public Guid Id { get; set; }
        [JsonProperty("HeadMaterials")]        
        public List<IHeadMaterial> HeadMaterials { get; } = [];
        [JsonProperty("ForceActions")]        
        public List<IForceAction> ForceActions { get; } = [];
        [JsonProperty("Primitives")]        
        public List<INdmPrimitive> Primitives { get; } = [];
        [JsonProperty("Calculators")]        
        public List<ICalculator> Calculators { get; } = [];
        [JsonIgnore]        
        public IRepositoryOperationsLogic Operations => operations ??= new RepositoryOperationsLogic(this);
    }
}
