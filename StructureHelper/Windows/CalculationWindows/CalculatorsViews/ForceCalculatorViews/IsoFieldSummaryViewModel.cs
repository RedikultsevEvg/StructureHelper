using FieldVisualizer.Entities.Values.Primitives;
using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    public class IsoFieldSummaryViewModel : ViewModelBase
    {
        private IPrimitiveSet primitiveSet;
        public double AreaTotal { get; private set; }
        public double AreaNeg { get; private set; }
        public double AreaZero { get; private set; }
        public double AreaPos { get; private set; }
        public double SumTotal { get; private set; }
        public double SumNeg { get; private set; }
        public double SumPos { get; private set; }
        public double MaxValue { get; private set; }
        public double MinValue { get; private set; }

        public IsoFieldSummaryViewModel(IPrimitiveSet primitiveSet)
        {
            this.primitiveSet = primitiveSet;
            Refresh();
        }

        public void Refresh()
        {
            AreaTotal = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Sum(x => x.Area);
            AreaNeg = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value < 0d).Sum(x => x.Area);
            AreaZero = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value == 0d).Sum(x => x.Area);
            AreaPos = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value > 0d).Sum(x => x.Area);
            SumTotal = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Sum(x => x.Value);
            SumNeg = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value < 0d).Sum(x => x.Value);
            SumPos = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Where(x => x.Value > 0d).Sum(x => x.Value);
            MaxValue = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Max(x => x.Value);
            MinValue = primitiveSet is null ? 0 : primitiveSet.ValuePrimitives.Min(x => x.Value);
            OnPropertyChanged(nameof(PrimitiveSet));
            OnPropertyChanged(nameof(AreaTotal));
            OnPropertyChanged(nameof(AreaNeg));
            OnPropertyChanged(nameof(AreaZero));
            OnPropertyChanged(nameof(AreaPos));
            OnPropertyChanged(nameof(SumTotal));
            OnPropertyChanged(nameof(SumNeg));
            OnPropertyChanged(nameof(SumPos));
            OnPropertyChanged(nameof(MaxValue));
            OnPropertyChanged(nameof(MinValue));
        }
    }
}
