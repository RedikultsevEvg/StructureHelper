﻿using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class CircleViewPrimitive : PrimitiveBase, IHasCenter
    {
        IEllipseNdmPrimitive primitive;
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

        public CircleViewPrimitive(INdmPrimitive primitive) : base(primitive)
        {
            if (primitive is not IEllipseNdmPrimitive)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $"\nExpected: {nameof(IEllipseNdmPrimitive)}, But was: {nameof(primitive)}");
            }
            var circle = primitive as IEllipseNdmPrimitive;
            this.primitive = circle;
            DivisionViewModel = new HasDivisionViewModel(circle.DivisionSize);
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
        public override void Refresh()
        {
            OnPropertyChanged(nameof(Diameter));
            OnPropertyChanged(nameof(PrimitiveLeft));
            OnPropertyChanged(nameof(PrimitiveTop));
            base.Refresh();
        }
    }
}
