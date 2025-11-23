using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    public class DeflectionFactorViewModel : ViewModelBase
    {
        IDeflectionFactor deflectionFactor;
        public ForceTupleVM DeflectionFactors { get; }
        public ForceTupleVM MaxDeflections { get; }

        public double SpanLength
        {
            get => deflectionFactor.SpanLength;
            set
            {
                deflectionFactor.SpanLength = Math.Max(value, 0.0);
                OnPropertyChanged(nameof(SpanLength));
            }
        }

        public DeflectionFactorViewModel(IDeflectionFactor deflectionFactor)
        {
            this.deflectionFactor = deflectionFactor;
            DeflectionFactors = new(this.deflectionFactor.DeflectionFactors)
            {
                MinMx = 0.0,
                MinMy = 0.0,
                MinNz = 0.0
            };
            MaxDeflections = new(this.deflectionFactor.MaxDeflections)
            {
                MinMx = 0.0,
                MinMy = 0.0,
                MinNz = 0.0
            };
        }
    }
}
