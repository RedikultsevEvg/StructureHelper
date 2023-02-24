using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class CircleViewPrimitive : PrimitiveBase, IHasDivision, IHasCenter
    {
        ICirclePrimitive primitive;
        public double Diameter
        { get
            {
                return primitive.Diameter;
            }
        set
            {
                primitive.Diameter = value;
                RefreshPlacement();
            }
        }
        public int NdmMinDivision { get; set; }
        public double NdmMaxSize { get; set; }

        public double PrimitiveLeft => DeltaX - Diameter / 2d;
        public double PrimitiveTop => DeltaY - Diameter / 2d;

        public CircleViewPrimitive(INdmPrimitive primitive) : base(primitive)
        {
            if (primitive is not ICirclePrimitive)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"\nExpected: {nameof(ICirclePrimitive)}, But was: {nameof(primitive)}");
            }
            this.primitive = primitive as ICirclePrimitive;
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
        }
    }
}
