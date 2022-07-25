using StructureHelper.Infrastructure.Enums;
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
            PrimitiveWidth = primitiveWidth;
            PrimitiveHeight = primitiveHeight;
            ShowedX = 0;
            ShowedY = 0;
        }

        public override INdmPrimitive GetNdmPrimitive()
        {
            double strength = 0;
            double centerX = 0;
            double centerY = 0;
            string materialName = "C20";
            ICenter center = new Center() { X = centerX, Y = centerY };
            double height = 0;
            double width = 0;
            IShape shape = new StructureHelperCommon.Models.Shapes.Rectangle() { Height = height, Width = width, Angle = 0 };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial() { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive() { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }
    }
}
