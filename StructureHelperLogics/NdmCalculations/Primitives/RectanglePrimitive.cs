using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
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
        public IHeadMaterial HeadMaterial { get; set; }
        public double PrestrainKx { get; set; }
        public double PrestrainKy { get; set; }
        public double PrestrainEpsZ { get; set; }
        public double NdmMaxSize { get; set; }
        public int NdmMinDivision { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Angle { get; set; }

        public RectanglePrimitive()
        {
            Name = "New Rectangle";
            NdmMaxSize = 0.01d;
            NdmMinDivision = 10;
        }

        public RectanglePrimitive(IHeadMaterial material) : this() { HeadMaterial = material; }

        public object Clone()
        {
            RectanglePrimitive primitive = new RectanglePrimitive();
            NdmPrimitivesService.CopyDivisionProperties(this, primitive);
            ShapeService.CopyRectangleProperties(this, primitive);
            return primitive;
        }

        public IEnumerable<INdm> GetNdms(IMaterial material)
        {
            List<INdm> ndms = new List<INdm>();
            var options = new RectangleTriangulationLogicOptions(this);
            ITriangulationLogic logic = new RectangleTriangulationLogic(options);
            ndms.AddRange(logic.GetNdmCollection(material));
            return ndms;
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
