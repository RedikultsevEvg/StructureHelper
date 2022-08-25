using System;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Point : PrimitiveBase
    {
        public Point(double d, double x, double y, MainViewModel mainViewModel) : base(PrimitiveType.Point, x, y, mainViewModel)
        {
            PrimitiveWidth = d;
            ShowedX = 0;
            ShowedY = 0;
        }

        public override INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem)
        {
            var width = unitSystem.ConvertLength(PrimitiveWidth);
            double area = Math.Round(width * width * Math.PI / 4, 2);
            string materialName = MaterialName;
            ICenter center = new Center { X = unitSystem.ConvertLength(ShowedX), Y = unitSystem.ConvertLength(ShowedY) };
            IShape shape = new StructureHelperCommon.Models.Shapes.Point { Area = area };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = Material.DesingTensileStrength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }
    }
}
