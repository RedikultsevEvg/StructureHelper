using StructureHelper.Infrastructure.Enums;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using System;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class RectangleViewPrimitive : PrimitiveBase, IHasDivision, IHasCenter
    {
        const double lengthUnit = 1000d; 

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
            get => DeltaX - primitive.Width / 2 * lengthUnit;
        }
        public double PrimitiveTop
        {
            get => DeltaY - primitive.Height / 2  * lengthUnit;
        }
        public int NdmMinDivision
        {
            get => primitive.NdmMinDivision;
            set
            {
                primitive.NdmMinDivision = value;
                OnPropertyChanged(nameof(NdmMinDivision));
            }
        }
        public double NdmMaxSize
        {
            get => primitive.NdmMaxSize;
            set
            {
                primitive.NdmMaxSize = value;
                OnPropertyChanged(nameof(NdmMaxSize));
            }
        }

        public RectangleViewPrimitive(IRectanglePrimitive _primitive) : base(_primitive)
        {
            primitive = _primitive;
        }

        public override INdmPrimitive GetNdmPrimitive()
        {           
            return primitive;
        }
    }
}
