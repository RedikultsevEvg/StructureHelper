using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using System;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;

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

        public override INdmPrimitive GetNdmPrimitive()
        {           
            return primitive;
        }
    }
}
