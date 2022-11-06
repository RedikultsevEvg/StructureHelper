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
        public double Area { get; set; }
        public Point(double area, double x, double y, MainViewModel ownerVm) : base(PrimitiveType.Point, x, y, ownerVm)
        {
            Name = "New point";
            Area = area;
            PreviewMouseMove = new RelayCommand(o =>
            {
                if (!(o is Point point)) return;
                if (point.Captured && !point.ElementLock)
                {
                    var pointDelta = point.PrimitiveWidth / 2;

                    if (point.ShowedX % 10 <= pointDelta || point.ShowedX % 10 >= 10 - pointDelta)
                        point.ShowedX = Math.Round((ownerVm.PanelX - OwnerVm.YX1) / 10) * 10;
                    else
                        point.ShowedX = ownerVm.PanelX - pointDelta - OwnerVm.YX1;

                    if (point.ShowedY % 10 <= pointDelta || point.ShowedY % 10 >= 10 - pointDelta)
                        point.ShowedY = -(Math.Round((ownerVm.PanelY - OwnerVm.XY1) / 10) * 10);
                    else
                        point.ShowedY = -(ownerVm.PanelY - pointDelta - OwnerVm.XY1);
                }
            });
            CenterX = x;
            CenterY = y;
        }

        public double Diameter { get => Math.Sqrt(Area / Math.PI) * 2; }

        public override INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem)
        {
            string materialName = MaterialName;
            ICenter center = new Center { X = CenterX, Y = CenterY };
            IShape shape = new StructureHelperCommon.Models.Shapes.Point { Area = this.Area };
            IPrimitiveMaterial primitiveMaterial = GetPrimitiveMaterial();
            //IPrimitiveMaterial primitiveMaterial = new PrimitiveMaterial { MaterialType = GetMaterialTypes(), ClassName = materialName, Strength = Material.DesignCompressiveStrength };
            INdmPrimitive ndmPrimitive = new NdmPrimitive { Center = center, Shape = shape, PrimitiveMaterial = primitiveMaterial,
                PrestrainKx = PrestrainKx,
                PrestrainKy = PrestrainKy,
                PrestrainEpsZ = PrestrainEpsZ
            };
            return ndmPrimitive;
        }
    }
}
