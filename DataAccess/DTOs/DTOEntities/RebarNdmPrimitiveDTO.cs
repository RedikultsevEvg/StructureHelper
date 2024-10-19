using LoaderCalculator.Data.Ndms;
using Newtonsoft.Json;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.DTOEntities
{
    public class RebarNdmPrimitiveDTO : IRebarNdmPrimitive
    {
        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonIgnore]
        public IShape Shape { get; set; }
        [JsonProperty("NdmElement")]
        public INdmElement NdmElement { get; set; } = new NdmElementDTO();
        [JsonIgnore]
        public ICrossSection? CrossSection { get; set; }
        [JsonProperty("VisualProperty")]
        public IVisualProperty VisualProperty { get; set; } = new VisualPropertyDTO();
        [JsonProperty("Center")]
        public IPoint2D Center { get; set; } = new Point2DDTO();
        [JsonProperty("RotationAngle")]
        public double RotationAngle { get; set; } = 0d;
        [JsonProperty("Area")]
        public double Area { get; set; } = 0d;


        public INdmPrimitive? HostPrimitive { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public Ndm GetConcreteNdm(ITriangulationOptions triangulationOptions)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            throw new NotImplementedException();
        }

        public RebarNdm GetRebarNdm(ITriangulationOptions triangulationOptions)
        {
            throw new NotImplementedException();
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            throw new NotImplementedException();
        }
    }
}
