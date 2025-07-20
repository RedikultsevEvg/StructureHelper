using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.Materials;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public class BeamShearSectionDTO : IBeamShearSection
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("Shape")]
        public IShape Shape { get; set; } = new RectangleShapeDTO(Guid.Empty);
        [JsonProperty("ConcreteMaterial")]
        public IConcreteLibMaterial ConcreteMaterial { get; set; } = new ConcreteLibMaterial(Guid.NewGuid());
        [JsonProperty("CenterCover")]
        public double CenterCover { get; set; }
        [JsonProperty("ReinforcementArea")]
        public double ReinforcementArea { get; set; }
        [JsonProperty("ReinforcementMaterial")]
        public IReinforcementLibMaterial ReinforcementMaterial { get; set; } = new ReinforcementLibMaterial(Guid.NewGuid());

        public BeamShearSectionDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }
    }
}
