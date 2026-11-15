using LoaderCalculator.Data.Ndms;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace DataAccess.DTOs
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
        public INdmElement NdmElement { get; set; } = new NdmElementDTO(Guid.Empty);
        [JsonIgnore]
        public ICrossSectionRepository? CrossSectionRepository { get; set; }
        [JsonProperty("VisualProperty")]
        public IVisualProperty VisualProperty { get; set; } = new VisualPropertyDTO();
        [JsonProperty("Center")]
        public IPoint2D Center { get; set; } = new Point2DDTO();
        [JsonProperty("RotationAngle")]
        public double RotationAngle { get; set; } = 0d;
        [JsonProperty("Area")]
        public double Area { get; set; } = 0d;


        public INdmPrimitive? HostPrimitive { get; set; }

        public RebarNdmPrimitiveDTO()
            : this(Guid.NewGuid())
        {
        }

        public RebarNdmPrimitiveDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            RebarNdmPrimitiveDTO newItem = new(Id);
            new NdmPrimitiveUpdateStrategy().Update(newItem, this);
            return newItem;
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

        public INdmPrimitive GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            RebarNdmPrimitiveDTO newItem = new(id);
            return newItem;
        }
    }
}
