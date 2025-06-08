using Newtonsoft.Json;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class CrossSectionRepositoryDTO : ICrossSectionRepository
    {
        [JsonProperty("Id")]        
        public Guid Id { get; set; }
        [JsonProperty("HeadMaterials")]        
        public List<IHeadMaterial> HeadMaterials { get; } = new();
        [JsonProperty("ForceActions")]        
        public List<IForceAction> ForceActions { get; } = new();
        [JsonProperty("Primitives")]        
        public List<INdmPrimitive> Primitives { get; } = new();
        [JsonProperty("Calculators")]        
        public List<ICalculator> Calculators { get; } = new();

    }
}
