using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoFieldSummary : ViewModelBase
    {
        private IPrimitiveSet primitiveSet;
        public double AreaTotal { get; private set; }
        public double AreaNeg { get; private set; }
        public double AreaZero { get; private set; }
        public double AreaPos { get; private set; }
        public double SumTotal { get; private set; }
        public double SumNeg { get; private set; }
        public double SumPos { get; private set; }

        public IsoFieldSummary(IPrimitiveSet primitiveSet)
        {
            this.primitiveSet = primitiveSet;
            Refresh();
        }

        private void Refresh()
        {
            AreaTotal = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Sum(x => x.Area);
            AreaNeg = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value < 0d).Sum(x => x.Area);
            AreaZero = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value == 0d).Sum(x => x.Area);
            AreaPos = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value > 0d).Sum(x => x.Area);
            SumTotal = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Sum(x => x.Value);
            SumNeg = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value < 0d).Sum(x => x.Value);
            SumPos = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value > 0d).Sum(x => x.Value);
            OnPropertyChanged(nameof(PrimitiveSet));
            OnPropertyChanged(nameof(AreaTotal));
            OnPropertyChanged(nameof(AreaNeg));
            OnPropertyChanged(nameof(AreaZero));
            OnPropertyChanged(nameof(AreaPos));
            OnPropertyChanged(nameof(SumTotal));
            OnPropertyChanged(nameof(SumNeg));
            OnPropertyChanged(nameof(SumPos));
        }
    }
}
