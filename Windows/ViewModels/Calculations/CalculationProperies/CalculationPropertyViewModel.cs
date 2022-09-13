using StructureHelper.Infrastructure;
using StructureHelperLogics.Infrastructures.CommonEnums;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.ViewModels.Calculations.CalculationProperies
{
    public class CalculationPropertyViewModel : ViewModelBase, IDataErrorInfo
    {
        public ObservableCollection<IForceCombination> ForceCombinations { get; private set; }
        public double IterationAccuracy
        {
            get
            {
                return calculationProperty.IterationProperty.Accuracy;
            }
            set
            {
                calculationProperty.IterationProperty.Accuracy = value;
                OnPropertyChanged("Accuracy");
            }
        }
        public int MaxIterationCount
        {
            get
            {
                return calculationProperty.IterationProperty.MaxIterationCount;
            }
            set
            {
                calculationProperty.IterationProperty.MaxIterationCount = value;
                OnPropertyChanged("MaxIterationCount");
            }
        }
        public IForceCombination SelectedCombination { get; set; }
        public LimitStates LimitState
        {
            get
            {
                return calculationProperty.LimitState;
            }
            set
            {
                calculationProperty.LimitState = value;
                OnPropertyChanged(nameof(LimitState));
            }
        }
        public CalcTerms CalcTerm
        {
            get
            {
                return calculationProperty.CalcTerm;
            }
            set
            {
                calculationProperty.CalcTerm = value;
                OnPropertyChanged(nameof(CalcTerm));
            }
        }

        public string Error => throw new NotImplementedException();
        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(IterationAccuracy))
                {
                    if (IterationAccuracy < 1e-10) { error = "Assigned accuracy of iterations is not valid"; }
                }
                else if (columnName == nameof(MaxIterationCount))
                {
                    if (MaxIterationCount < 2) { error = "Number of iterations should be greater than 1"; }
                }
                return error;
            }
        }

        public ICommand AddForceCombinationCommand { get; private set; }
        public ICommand RemoveForceCombinationCommand { get; private set; }

        private readonly ICalculationProperty calculationProperty;
        public CalculationPropertyViewModel(ICalculationProperty calculationProperty)
        {
            this.calculationProperty = calculationProperty;
            ForceCombinations = new ObservableCollection<IForceCombination>();
            foreach (var force in calculationProperty.ForceCombinations)
            {
                ForceCombinations.Add(force);
            }
            AddForceCombinationCommand = new RelayCommand(o => AddForceCombination());
            RemoveForceCombinationCommand = new RelayCommand(o => RemoveForceCombination(), o => SelectedCombination != null); 
        }

        public void SaveProperties()
        {
            calculationProperty.ForceCombinations.Clear();
            foreach (var force in ForceCombinations)
            {
                calculationProperty.ForceCombinations.Add(force);
            }
        }
        private void AddForceCombination()
        {
            ForceCombinations.Add(new ForceCombination());
        }
        private void RemoveForceCombination()
        {
            ForceCombinations.Remove(SelectedCombination);
        }
    }
}
