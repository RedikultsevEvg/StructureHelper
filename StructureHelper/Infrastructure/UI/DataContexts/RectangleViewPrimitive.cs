using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class RectangleViewPrimitive : PrimitiveBase, IHasCenter
    {
        private IRectanglePrimitive primitive;

        public override double PrimitiveWidth
        {
            get => primitive.Width;
            set
            {
                primitive.Width = value;
                OnPropertyChanged(nameof(PrimitiveLeft));
                OnPropertyChanged(nameof(PrimitiveWidth));
            }
        }
        public override double PrimitiveHeight
        {
            get => primitive.Height;
            set
            {
                primitive.Height = value;
                OnPropertyChanged(nameof(PrimitiveTop));
                OnPropertyChanged(nameof(PrimitiveHeight));
            }
        }

        public double PrimitiveLeft
        {
            get => DeltaX - primitive.Width / 2d;
        }
        public double PrimitiveTop
        {
            get => DeltaY - primitive.Height / 2d;
        }

        public RectangleViewPrimitive(IRectanglePrimitive _primitive) : base(_primitive)
        {
            primitive = _primitive;
            DivisionViewModel = new HasDivisionViewModel(primitive);
        }
        public override void Refresh()
        {
            OnPropertyChanged(nameof(PrimitiveLeft));
            OnPropertyChanged(nameof(PrimitiveTop));
            base.Refresh();
        }
        public override INdmPrimitive GetNdmPrimitive()
        {           
            return primitive;
        }
    }
}
