using System;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Point : PrimitiveBase
    {
        private double square;
        public double Square
        {
            get => square;
            set
            {
                square = value;
                PrimitiveWidth = Math.Round(Math.Sqrt(4 * value / Math.PI), 2);
                OnPropertyChanged(nameof(PrimitiveWidth));
                OnPropertyChanged();
            }
        }

        public Point(double square, double x, double y, MainViewModel mainViewModel) : base(PrimitiveType.Point, x, y, mainViewModel)
        {
            Square = square;
            ShowedX = 0;
            ShowedY = 0;
        }

        public override INdmPrimitive GetNdmPrimitive()
        {
            double strength = 0;
            double centerX = 0;
            double centerY = 0;
            double area = 0;
            string materialName = "s400";
            ICenter center = new Center { X = centerX, Y = centerY };
            IShape shape = new StructureHelperCommon.Models.Shapes.Point { Area = area };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }
    }
}
