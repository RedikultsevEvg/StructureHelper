using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Rectangle : PrimitiveBase
    {
        public Rectangle(double primitiveWidth, double primitiveHeight, double rectX, double rectY, MainViewModel mainViewModel) : base(PrimitiveType.Rectangle, rectX, rectY, mainViewModel)
        {
            Type = PrimitiveType.Rectangle;
            PrimitiveWidth = primitiveWidth;
            PrimitiveHeight = primitiveHeight;
            ShowedX = 0;
            ShowedY = 0;
        }

        public override INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem)
        {
            var width = unitSystem.ConvertLength(PrimitiveWidth);
            var height = unitSystem.ConvertLength(PrimitiveHeight);
            double centerX = unitSystem.ConvertLength(ShowedX) + width / 2;
            double centerY = unitSystem.ConvertLength(ShowedY) + height / 2;
            string materialName = MaterialName;
            ICenter center = new Center { X = centerX, Y = centerY };
            IShape shape = new StructureHelperCommon.Models.Shapes.Rectangle { Height = height, Width = width, Angle = 0 };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = Material.DesingTensileStrength };
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }
    }
}
