using Newtonsoft.Json;
using StructureHelperCommon.Models.WorkPlanes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class WorkPlanePropertyDTO : IWorkPlaneProperty
    {
        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("GridSize")]
        public double GridSize { get; set; } = 0.05;
        [JsonProperty("Width")]
        public double Width { get; set; } = 1.2;
        [JsonProperty("Height")]
        public double Height { get; set; } = 1.2;
        [JsonProperty("AxisLineThickness")]
        public double AxisLineThickness { get; set; } = 2;
        [JsonProperty("GridLineThickness")]
        public double GridLineThickness { get; set; } = 0.25;

        public WorkPlanePropertyDTO(Guid id)
        {
            Id = id;
        }
    }
}
