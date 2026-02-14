using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class CircleViewPrimitive : PrimitiveBase, IHasCenter
    {
        private IEllipseNdmPrimitive primitive;
        private IDigitRoundLogic digitRoundLogic = new SmartRoundLogic();
        public double Diameter
        {
            get
            {
                return primitive.Width;
            }
            set
            {
                primitive.Width = value;
                RefreshPlacement();
            }
        }

        public double PrimitiveLeft => DeltaX - Diameter / 2d;
        public double PrimitiveTop => DeltaY - Diameter / 2d;

        public double Area => digitRoundLogic.RoundValue(Math.PI * Diameter * Diameter / 4.0);

        public CircleViewPrimitive(IEllipseNdmPrimitive primitive) : base(primitive)
        {
            this.primitive = primitive;
            DivisionViewModel = new HasDivisionViewModel(primitive.DivisionSize);
        }

        public override INdmPrimitive GetNdmPrimitive()
        {
            return primitive;
        }

        private void RefreshPlacement()
        {
            OnPropertyChanged(nameof(Diameter));
            OnPropertyChanged(nameof(CenterX));
            OnPropertyChanged(nameof(CenterY));
            OnPropertyChanged(nameof(PrimitiveLeft));
            OnPropertyChanged(nameof(PrimitiveTop));
            OnPropertyChanged(nameof(Area));
        }
        public override void Refresh()
        {
            OnPropertyChanged(nameof(Diameter));
            OnPropertyChanged(nameof(PrimitiveLeft));
            OnPropertyChanged(nameof(PrimitiveTop));
            base.Refresh();
        }
    }
}
