using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Models.Entities;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using System;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Rectangle : PrimitiveBase
    {
        public Rectangle(double primitiveWidth, double primitiveHeight, double x, double y, MainViewModel ownerVm) : base(PrimitiveType.Rectangle, x, y, ownerVm)
        {
            Type = PrimitiveType.Rectangle;
            Name = "New rectangle";
            PrimitiveWidth = primitiveWidth;
            PrimitiveHeight = primitiveHeight;
            PreviewMouseMove = new RelayCommand(o =>
            {
                if (!(o is Rectangle rect)) return;
                if (Captured && !rect.BorderCaptured && !ElementLock)
                {
                    var deltaX = PrimitiveWidth / 2;
                    var deltaY = PrimitiveHeight / 2;

                    if (rect.ShowedX % 10 <= delta || rect.ShowedX % 10 >= 10 - delta)
                        rect.ShowedX = Math.Round((OwnerVm.PanelX - deltaX - OwnerVm.YX1) / 10) * 10;
                    else
                        rect.ShowedX = OwnerVm.PanelX - deltaX - OwnerVm.YX1;

                    if (rect.ShowedY % 10 <= delta || rect.ShowedY % 10 >= 10 - delta)
                        rect.ShowedY = -(Math.Round((OwnerVm.PanelY - deltaY - OwnerVm.XY1 + rect.PrimitiveHeight) / 10) * 10);
                    else
                        rect.ShowedY = -(OwnerVm.PanelY - deltaY - OwnerVm.XY1 + rect.PrimitiveHeight);
                }
            });
            CenterX = x;
            CenterY = y;
            MinElementDivision = 10;
            MaxElementSize = Math.Min(Math.Min(PrimitiveWidth, PrimitiveHeight) / MinElementDivision, 0.01);
        }

        public override INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem)
        {
            var width = PrimitiveWidth;
            var height = PrimitiveHeight;
            double centerX = CenterX;
            double centerY = CenterY;
            string materialName = MaterialName;
            ICenter center = new Center { X = centerX, Y = centerY };
            IShape shape = new StructureHelperCommon.Models.Shapes.Rectangle { Height = height, Width = width, Angle = 0 };
            IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = Material.DesignCompressiveStrength };
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial, NdmMaxSize = MaxElementSize, NdmMinDivision = MinElementDivision };
            return ndmPrimitive;
        }
    }
}
