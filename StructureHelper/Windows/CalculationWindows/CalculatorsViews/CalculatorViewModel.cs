using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class CalculatorViewModel : ViewModelBase
    {
        private ICalculator calcualtor;

        public bool ShowTraceData
        {
            get => calcualtor.ShowTraceData;
            set
            {
                calcualtor.ShowTraceData = value;
                OnPropertyChanged(nameof(ShowTraceData));
            }
        }
        public string Name
        {
            get => calcualtor.Name;
            set
            {
                calcualtor.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public CalculatorViewModel(ICalculator calcualtor)
        {
            this.calcualtor = calcualtor;
        }
    }
}
