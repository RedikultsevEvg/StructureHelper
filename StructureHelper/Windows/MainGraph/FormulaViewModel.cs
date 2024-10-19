
using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace StructureHelper.Windows.MainGraph
{
    public class FormulaViewModel : ViewModelBase
    {
        private const string DEFAULT_NAME = "Put function name here...";
        private const string DEFAULT_DESCRIPTION = "Put function description here...";
        private const string DEFAULT_FORMULA = "x^2";
        private const double DEFAULT_LEFT_BOUND = 0;
        private const double DEFAULT_RIGHT_BOUND = 1000;
        private const int DEFAULT_STEP = 100;
        private RelayCommand drawGraphCommand;
        public ICommand DrawGraphCommand
        {
            get => drawGraphCommand ??= new RelayCommand(o => Save(o));
        }
        private string formula;

        public string Formula
        {
            get => formula;
            set
            {
                formula = value;
            }
        }
        private string formulaText = "y(x)=";
        public string FormulaText
        {
            get => formulaText;
            set
            {
                formulaText = $"y(x)={Formula}";
                OnPropertyChanged(nameof(Formula)); 
            }
        }
        private IOneVariableFunction function;
        public IOneVariableFunction Function
        {
            get => function;
            set
            {
                function = value;
            }
        }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
            }
        }
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
            }
        }
        public FormulaViewModel()
        {
            Name = DEFAULT_NAME;
            Description = DEFAULT_DESCRIPTION;
        }
        public FormulaViewModel(FormulaFunction formulaFunction)
        {
            Function = formulaFunction;
            Name = Function.Name;
            Description = Function.Description;
        }
        private void Save(object parameter)
        {
            if (Function is null)
            {
                Function = new FormulaFunction();
            }
            Function.Name = Name;
            Function.Description = Description;
            Function.IsUser = true;
            (Function as FormulaFunction).Formula = Formula;
            var window = parameter as Window;
            window.DialogResult = true;
            window.Close();
        }

    }
}
