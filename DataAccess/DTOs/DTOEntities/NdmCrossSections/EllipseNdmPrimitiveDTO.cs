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
    public class EllipseNdmPrimitiveDTO : IEllipseNdmPrimitive
    {
        private IEllipseShape shape = new EllipseShapeDTO(Guid.Empty);

        public EllipseNdmPrimitiveDTO(Guid id)
        {
            Id = id;
        }

        [JsonProperty("Id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string? Name { get; set; }
        [JsonProperty("RectangleShape")]
        public IEllipseShape EllipseShape
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
        public double Height {get; set; }
        [JsonIgnore]
        public ICrossSectionRepository? CrossSectionRepository { get; set; }

        public object Clone()
        {
            return this;
        }

        public INdmPrimitive GetClone(CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            if (cloneMode == CloneMode.Reference) { return this; }
            Guid id = GuidService.GetGuid(this, cloneMode);
            EllipseNdmPrimitiveDTO newItem = new(id);
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
    }
}
