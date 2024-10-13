using Newtonsoft.Json;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DivisionSizeDTO : IDivisionSize
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("NdmMaxSize")]
        public double NdmMaxSize { get; set; }
        [JsonProperty("NdmMinDivision")]
        public int NdmMinDivision { get; set; }
        [JsonProperty("ClearUnderlying")]
        public bool ClearUnderlying { get; set; }

    }
}
