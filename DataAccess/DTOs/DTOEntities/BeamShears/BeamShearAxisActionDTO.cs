using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearAxisActionDTO : IBeamShearAxisAction
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [JsonProperty("SupportForce")]
        public IFactoredForceTuple SupportForce { get; set; }
        [JsonProperty("SpanLoads")]
        public List<IBeamSpanLoad> ShearLoads { get; } = new();

        public BeamShearAxisActionDTO(Guid id)
        {
            Id = id;
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
