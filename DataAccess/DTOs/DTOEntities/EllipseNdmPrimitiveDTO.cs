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

namespace DataAccess.DTOs
{
    public class EllipseNdmPrimitiveDTO : IEllipseNdmPrimitive
    {
        private IRectangleShape shape = new RectangleShapeDTO();

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
        public INdmElement NdmElement { get; set; } = new NdmElementDTO();
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
        public ICrossSection? CrossSection { get; set; }

        public object Clone()
        {
            throw new NotImplementedException();
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
