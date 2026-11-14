using LoaderCalculator.Data.Ndms;
using Newtonsoft.Json;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;

namespace DataAccess.DTOs
{
    public class RectangleNdmPrimitiveDTO : IRectangleNdmPrimitive
    {
        private IRectangleShape shape = new RectangleShapeDTO(Guid.Empty);

        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("RectangleShape")]
        public IRectangleShape RectangleShape
        {
            get => shape;
            set => shape = value;
        }
        [JsonIgnore]
        public IShape Shape => shape;
        [JsonProperty("NdmElement")]
        public INdmElement NdmElement { get; set; } = new NdmElementDTO(Guid.Empty);
        [JsonProperty("VisualProperty")]
        public IVisualProperty VisualProperty { get; set; } = new VisualPropertyDTO();
        [JsonProperty("Center")]
        public IPoint2D Center { get; set; } = new Point2DDTO();
        [JsonProperty("DivisionSize")]
        public IDivisionSize DivisionSize { get; set; } = new DivisionSizeDTO();
        [JsonProperty("RotationAngle")]
        public double RotationAngle { get; set; }
        [JsonIgnore]
        public double Width { get; set; }
        [JsonIgnore]
        public double Height { get; set; }
        [JsonIgnore]
        public ICrossSection? CrossSection { get; set; }

        public RectangleNdmPrimitiveDTO()
            : this(Guid.NewGuid())
        {
        }

        public RectangleNdmPrimitiveDTO(Guid id)
        {
            Id = id;
        }

        public object Clone()
        {
            RectangleNdmPrimitiveDTO newItem = new RectangleNdmPrimitiveDTO(Id);
            new NdmPrimitiveUpdateStrategy().Update(newItem, this);
            return newItem;
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

        public INdmPrimitive GetClone(INdmPrimitive sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
