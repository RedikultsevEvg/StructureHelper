using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StructureHelper.Windows.Forces
{
    public class FactoredCombinationPropertyVM : ViewModelBase, IDataErrorInfo
    {
        private IFactoredCombinationProperty sourceProperty;

        public List<LimitStates> LimitStateList { get; set; } = new() { LimitStates.ULS, LimitStates.SLS };
        public List<CalcTerms> CalcTermList { get; set; } = new() { CalcTerms.LongTerm, CalcTerms.ShortTerm };

        public FactoredCombinationPropertyVM(IFactoredCombinationProperty sourceProperty)
        {
            this.sourceProperty = sourceProperty;
        }

        public double ULSFactor
        {
            get => sourceProperty.ULSFactor;
            set
            {
                sourceProperty.ULSFactor = value;
                OnPropertyChanged(nameof(ULSFactor));
            }
        }

        public double LongTermFactor
        {
            get => sourceProperty.LongTermFactor;
            set
            {
                if (value < 0d) { value = 0d; }
                if (value > 1d) { value = 1d; }
                sourceProperty.LongTermFactor = value;
                OnPropertyChanged(nameof(LongTermFactor));
            }
        }

        public CalcTerms CalcTerm
        {
            get
            {
                return sourceProperty.CalcTerm;
            }
            set
            {
                sourceProperty.CalcTerm = value;
                OnPropertyChanged(nameof(CalcTerm));
            }
        }
        public LimitStates LimitState
        {
            get { return sourceProperty.LimitState; }
            set
            {
                sourceProperty.LimitState = value;
                OnPropertyChanged(nameof(LimitState));
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = null;
                if (columnName == nameof(ULSFactor))
                {
                    if (ULSFactor <= 0)
                    {
                        error = "Safety factor for ULS must be greater than zero";
                    }
                }
                if (columnName == nameof(LongTermFactor))
                {
                    if (LongTermFactor < 0d || LongTermFactor > 1d)
                    {
                        error = "Long term factor must be between 0.0 and 1.0";
                    }
                }
                return error;
            }
        }
    }
}
