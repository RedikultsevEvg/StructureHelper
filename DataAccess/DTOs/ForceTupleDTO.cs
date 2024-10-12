using Newtonsoft.Json;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class ForceTupleDTO : IForceTuple
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Mx")]
        public double Mx { get; set; }
        [JsonProperty("My")]
        public double My { get; set; }
        [JsonProperty("Nz")]
        public double Nz { get; set; }
        [JsonProperty("Qx")]
        public double Qx { get; set; }
        [JsonProperty("Qy")]
        public double Qy { get; set; }
        [JsonProperty("Mz")]
        public double Mz { get; set; }


        public void Clear()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
