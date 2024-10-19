using System;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class PointViewPrimitive : PrimitiveBase, IHasCenter
    {
        IPointNdmPrimitive primitive;

        public double Area
        { get => primitive.Area;
          set
            {
                primitive.Area = value;
                RefreshPlacement();
            }
        }

        public double PrimitiveLeft
        {
            get => DeltaX - Diameter / 2d;
        }
        public double PrimitiveTop
        {
            get => DeltaY - Diameter / 2d;
        }

        public PointViewPrimitive(IPointNdmPrimitive _primitive) : base(_primitive)
        {
            primitive = _primitive;
        }

        public double Diameter { get => Math.Sqrt(primitive.Area / Math.PI) * 2; }

        public override INdmPrimitive GetNdmPrimitive()
        {
            return primitive;
        }
        public override void Refresh()
        {
            RefreshPlacement();
            base.Refresh();
        }
        private void RefreshPlacement()
        {
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Diameter));
            OnPropertyChanged(nameof(CenterX));
            OnPropertyChanged(nameof(CenterY));
            OnPropertyChanged(nameof(PrimitiveLeft));
            OnPropertyChanged(nameof(PrimitiveTop));
        }
    }
}
