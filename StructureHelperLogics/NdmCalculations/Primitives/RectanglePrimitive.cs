using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
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
    public class RectanglePrimitive : INdmPrimitive, IRectangleShape, IHasDivisionSize, ISaveable, ICloneable
    {
        public string Name { get; set; }
        public ICenter Center { get; set; }
        public IShape Shape { get; set; }
        public IHeadMaterial HeadMaterial { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public int Id { get; set; }

        public double Width { get; set; }
        public double Height { get; set; }
        public double Angle { get; set; }

        public object Clone()
        {
            RectanglePrimitive primitive = new RectanglePrimitive();
            NdmPrimitivesService.CopyNdmProperties(this, primitive);
            NdmPrimitivesService.CopyDivisionProperties(this, primitive);
            ShapeService.CopyRectangleProperties(this, primitive);
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
