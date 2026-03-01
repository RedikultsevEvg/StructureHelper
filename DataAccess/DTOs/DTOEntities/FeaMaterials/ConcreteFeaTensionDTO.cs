using Newtonsoft.Json;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class ConcreteFeaTensionDTO : IConcreteFeaTension
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Strength")]
        public double Strength { get; set; }
        [JsonProperty("FractureEnergy")]
        public double FractureEnergy { get; set; }
        [JsonProperty("FeSize")]
        public double FeSize { get; set; }


        public ConcreteFeaTensionDTO(Guid id)
        {
            Id = id;
        }
    }
}
