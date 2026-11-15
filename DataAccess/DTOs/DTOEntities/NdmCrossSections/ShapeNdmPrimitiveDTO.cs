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
    public class ShapeNdmPrimitiveDTO : IShapeNdmPrimitive
    {
        private IShape shape;

        [JsonProperty("Id")]
        public Guid Id { get; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("Shape")]
        public IShape Shape { get => shape; set => shape = value; }
        [JsonProperty("NdmElement")]
        public INdmElement NdmElement { get; set; }
        [JsonProperty("Center")]
        public IPoint2D Center { get; set; }
        [JsonProperty("VisualProperty")]
        public IVisualProperty VisualProperty { get; set; }
        [JsonProperty("RotationAngle")]
        public double RotationAngle { get; set; }
        [JsonProperty("DivisionSize")]
        public IDivisionSize DivisionSize { get; set; }
        [JsonIgnore]
        public ICrossSectionRepository? CrossSectionRepository { get; set; }
        public ShapeNdmPrimitiveDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            return this;
        }

        public IEnumerable<INdm> GetNdms(ITriangulationOptions triangulationOptions)
        {
            throw new NotImplementedException();
        }

        public List<INamedAreaPoint> GetValuePoints()
        {
            throw new NotImplementedException();
        }

        public bool IsPointInside(IPoint2D point)
        {
            throw new NotImplementedException();
        }

        public void SetShape(IShape shape)
        {
            this.shape = shape;
        }

        public INdmPrimitive GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            ShapeNdmPrimitiveDTO newItem = new(id);
            return newItem;
        }
    }
}
