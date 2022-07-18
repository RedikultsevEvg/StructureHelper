using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using StructureHelperLogics.NdmCalculations.Materials;

namespace StructureHelperLogics.Models.NdmPrimitives
{
    public class RectanglePrimitive : PrimitiveBase<IRectangle>, IRectangle
    {
        public RectanglePrimitive(ICenter center, IRectangle shape) : base(center, shape) { }

        public double Width => _shape.Width;

        public double Height => _shape.Height;

        public double Angle => _shape.Angle;

        public override INdmPrimitive GetNdmPrimitive()
        {
            double strength = 400;
            string materialName = "s400";
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial() { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive() { Center = _center, Shape = _shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }

        private MaterialTypes GetMaterialTypes()
        {
            return MaterialTypes.Reinforcement;
        }
    }
}
