using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Windows;
using System.Windows.Media;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class RingShapeViewPrimitive : PrimitiveBase, IHasCenter
    {
        private IShapeNdmPrimitive primitive;
        private Geometry _ringGeometry;

        private IRingShape OShape => (IRingShape)primitive.Shape;



        public double OuterDiameter
        {
            get => OShape.OuterDiameter;
            set
            {
                if (value > InnerDiameter)
                {
                    OShape.OuterDiameter = value;
                    RefreshPlacement();
                }
            }
        }

        public double InnerDiameter
        {
            get => OShape.InnerDiameter;
            set
            {
                if (value > 0 && value < OuterDiameter)
                {
                    OShape.InnerDiameter = value;
                    RefreshPlacement();
                }
            }
        }
        public double OuterRadius => OShape.OuterRadius;
        public double InnerRadius => OShape.InnerRadius;

        public double Area => Math.Round(Math.PI * (OuterDiameter * OuterDiameter - InnerDiameter * InnerDiameter) / 4.0, 6);

        public double PrimitiveLeft => DeltaX - OuterDiameter / 2d;
        public double PrimitiveTop => DeltaY - OuterDiameter / 2d;

        public Geometry RingGeometry
        {
            get => _ringGeometry;
            private set
            {
                _ringGeometry = value;
                OnPropertyChanged();
            }
        }

        private void RebuildGeometry()
        {
            var center = new Point(0, 0);
            RingGeometry = new CombinedGeometry(
                GeometryCombineMode.Exclude,
                new EllipseGeometry(center, OuterRadius, OuterRadius),
                new EllipseGeometry(center, InnerRadius, InnerRadius));
        }

        public RingShapeViewPrimitive(IShapeNdmPrimitive primitive) : base(primitive)
        {
            this.primitive = primitive;
            DivisionViewModel = new HasDivisionViewModel(primitive.DivisionSize);
        }

        public override void Refresh()
        {
            RefreshPlacement();
            base.Refresh();
        }

        private void RefreshPlacement()
        {
            OnPropertyChanged(nameof(OuterDiameter));
            OnPropertyChanged(nameof(InnerDiameter));
            OnPropertyChanged(nameof(OuterRadius));
            OnPropertyChanged(nameof(InnerRadius));
            OnPropertyChanged(nameof(CenterX));
            OnPropertyChanged(nameof(CenterY));
            OnPropertyChanged(nameof(InvertedCenterY));
            OnPropertyChanged(nameof(Center));
            OnPropertyChanged(nameof(InvertedCenter));
            OnPropertyChanged(nameof(Area));
            RebuildGeometry();
            OnPropertyChanged(nameof(RingGeometry));
            OnPropertyChanged(nameof(PrimitiveTop));
            OnPropertyChanged(nameof(PrimitiveLeft));
        }
    }
}
