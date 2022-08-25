using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.NdmPrimitives
{
    public class PointPrimitive : PrimitiveBase<IPoint>, IPoint
    {
        public double Area
        {
            get => _shape.Area;
            set => _shape.Area = value;
        }

        public PointPrimitive(ICenter center, IPoint shape) : base(center, shape) { }
        public override INdmPrimitive GetNdmPrimitive()
        {
            double strength = 400e6d;
            string materialName = "s400";
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = _center, Shape = _shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }

        private MaterialTypes GetMaterialTypes()
        {
            return MaterialTypes.Reinforcement;
        }
    }
}
