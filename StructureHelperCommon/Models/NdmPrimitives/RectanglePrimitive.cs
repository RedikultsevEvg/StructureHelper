using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.NdmPrimitives
{
    public class RectanglePrimitive : PrimitiveBase<IRectangle>, IRectangle
    {
        public RectanglePrimitive(ICenter center, IRectangle shape) : base(center, shape) { }

        public double Width => _shape.Width;

        public double Height => _shape.Height;

        public double Angle => _shape.Angle;

        public override INdmPrimitive GetNdmPrimitive()
        {
            double strength = 40e6d;
            string materialName = "C40/45";
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = _center, Shape = _shape, PrimitiveMaterial = primitiveMaterial, NdmMaxSize = 1, NdmMinDivision = 20 };
            return ndmPrimitive;
        }

        private MaterialTypes GetMaterialTypes()
        {
            return MaterialTypes.Concrete;
        }
    }
}
