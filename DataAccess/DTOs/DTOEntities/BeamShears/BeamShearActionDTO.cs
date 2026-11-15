using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearActionDTO : IBeamShearAction
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("ExternalForce")]
        public IFactoredForceTuple ExternalForce { get; set; } = new FactoredForceTupleDTO(Guid.Empty);
        [JsonProperty("SupportAction")]
        public IBeamShearAxisAction SupportAction { get; set; } = new BeamShearAxisActionDTO(Guid.Empty);

        public BeamShearActionDTO(Guid id)
        {
            Id = id;
        }

        public IBeamShearAction GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            BeamShearActionDTO newItem = new(id);
            return newItem;
        }
    }
}
