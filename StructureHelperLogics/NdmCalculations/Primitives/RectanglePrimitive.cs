using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ShapeServices;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class RectanglePrimitive : IRectanglePrimitive
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public IHeadMaterial? HeadMaterial { get; set; }
        public IStrainTuple UsersPrestrain { get; private set; }
        public IStrainTuple AutoPrestrain { get; private set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Angle { get; set; }
        public bool ClearUnderlying { get; set; }
        public bool Triangulate { get; set; }
        public IVisualProperty VisualProperty { get; }


        public RectanglePrimitive()
        {
            Name = "New Rectangle";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
            VisualProperty = new VisualProperty { Opacity = 0.8d};
            UsersPrestrain = new StrainTuple();
            AutoPrestrain = new StrainTuple();
            ClearUnderlying = false;
            Triangulate = true;
        }

        public RectanglePrimitive(IHeadMaterial material) : this() { HeadMaterial = material; }

        public object Clone()
        {
            var primitive = new RectanglePrimitive();
            NdmPrimitivesService.CopyNdmProperties(this, primitive);
            NdmPrimitivesService.CopyDivisionProperties(this, primitive);
            ShapeService.CopyRectangleProperties(this, primitive);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            var ndms = new List<INdm>();
            var options = new RectangleTriangulationLogicOptions(this);
            var logic = new RectangleTriangulationLogic(options);
            ndms.AddRange(logic.GetNdmCollection(material));
            return ndms;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public bool IsPointInside(IPoint2D point)
        {
            var xMax = CenterX + Width / 2;
            var xMin = CenterX - Width / 2;
            var yMax = CenterY + Height / 2;
            var yMin = CenterY - Height / 2;
            if (point.X > xMax ||
                point.X < xMin ||
                point.Y > yMax ||
                point.Y < yMin)
            { return false; }
            return true;
        }
    }
}
