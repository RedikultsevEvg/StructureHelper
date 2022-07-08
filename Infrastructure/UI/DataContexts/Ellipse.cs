using System;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Data.Shapes;
using StructureHelperLogics.NdmCalculations.Entities;
using StructureHelperLogics.NdmCalculations.Materials;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Ellipse : PrimitiveBase
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

        public Ellipse(double square, double ellipseX, double ellipseY, MainViewModel mainViewModel) : base(PrimitiveType.Ellipse, ellipseX, ellipseY, mainViewModel)
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
            ICenter center = new Center() { X = centerX, Y = centerY };
            IShape shape = new Point() { Area = area };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial() { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = strength }; ;
            INdmPrimitive ndmPrimitive = new NdmPrimitive() { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial };
            return ndmPrimitive;
        }
    }
}
