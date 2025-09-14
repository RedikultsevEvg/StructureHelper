using StructureHelperLogics.NdmCalculations.Primitives;
using System;

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
