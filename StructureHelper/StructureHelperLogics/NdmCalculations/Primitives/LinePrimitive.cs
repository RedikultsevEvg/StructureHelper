using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ShapeServices;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class LinePrimitive : ILinePrimitive
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }

        public IPoint2D StartPoint { get; set; }
        public IPoint2D EndPoint { get; set; }
        public double Thickness { get; set; }

        public IVisualProperty VisualProperty => throw new NotImplementedException();

        public IStrainTuple UsersPrestrain => throw new NotImplementedException();

        public IStrainTuple AutoPrestrain => throw new NotImplementedException();

        public LinePrimitive()
        {
            StartPoint = new Point2D();
            EndPoint = new Point2D();

            Name = "New Line";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
        }

        public object Clone()
        {
            var primitive = new LinePrimitive();
            NdmPrimitivesService.CopyDivisionProperties(this, primitive);
            ShapeService.CopyLineProperties(this, primitive);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
