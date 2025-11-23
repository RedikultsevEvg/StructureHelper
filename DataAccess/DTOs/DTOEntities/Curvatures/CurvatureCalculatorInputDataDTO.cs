using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DTOs
{
    public class CurvatureCalculatorInputDataDTO : ICurvatureCalculatorInputData
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("DesignFactor")]
        public IDeflectionFactor DeflectionFactor { get; set; }
        [JsonProperty("ForceActions")]
        public List<IForceAction> ForceActions { get; } = [];
        [JsonProperty("Primitives")]
        public List<INdmPrimitive> Primitives { get; } = [];
        public CurvatureCalculatorInputDataDTO(Guid id)
        {
            Id = id;
        }
    }
}
