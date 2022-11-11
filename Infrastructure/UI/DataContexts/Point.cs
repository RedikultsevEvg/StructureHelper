using System;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class Point : PrimitiveBase
    {
        private double area;
        public double Area
        { get => area;
          set
            {
                area = value;
                OnPropertyChanged(nameof(Area));
                OnPropertyChanged(nameof(Diameter));
            }
        }
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

        public double Diameter { get => Math.Sqrt(area / Math.PI) * 2; }

        public override INdmPrimitive GetNdmPrimitive(IUnitSystem unitSystem)
        {
            string materialName = MaterialName;
            ICenter center = new Center { X = CenterX, Y = CenterY };
            IShape shape = new StructureHelperCommon.Models.Shapes.Point { Area = this.Area };
            INdmPrimitive ndmPrimitive = new NdmPrimitive(HeadMaterial)
            { Center = center, Shape = shape,
                PrestrainKx = PrestrainKx,
                PrestrainKy = PrestrainKy,
                PrestrainEpsZ = PrestrainEpsZ
            };
            return ndmPrimitive;
        }
    }
}
