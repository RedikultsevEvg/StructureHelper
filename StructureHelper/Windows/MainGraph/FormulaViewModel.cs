
using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.MainGraph
{
    public class FormulaViewModel : ViewModelBase
    {
        private const string ERROR_BOUNDS = "The left bound must be less than the right bound";
        private const string ERROR_STEP = "The number of steps should not be more than";
        private const string DEFAULT_NAME = "Put function name here...";
        private const string DEFAULT_DESCRIPTION = "Put function description here...";
        private const string DEFAULT_FORMULA = "x^2";
        private const double DEFAULT_LEFT_BOUND = 1;
        private const double DEFAULT_RIGHT_BOUND = 1000;
        private const int DEFAULT_STEP = 100;
        private const int MAX_STEP = 1000;
        public char GREATER { get; } = '\u2265';
        public char LESS { get; } = '\u2264';
        public char X { get; } = 'x';
        private RelayCommand saveCommand;
        private RelayCommand editColor;
        public int Step { get; set; }
        private double leftBound;
        public double LeftBound 
        { 
            get => leftBound;
            set
            {
                leftBound = value;
                LimitText = $"x\u2208[{value};{RightBound}]";
                OnPropertyChanged(nameof(LeftBound));
            }
        }
        private double rightBound;
        public double RightBound 
        {
            get => rightBound;
            set
            {
                rightBound = value;
                LimitText = $"x\u2208[{LeftBound};{value}]";
                OnPropertyChanged(nameof(RightBound));
            }
        }
        public ObservableCollection<Color> Colors { get; set; }
        public ICommand EditColorCommand
        {
            get => editColor ??= new RelayCommand(o => EditColor());
        }
        public ICommand SaveCommand
        {
            get => saveCommand ??= new RelayCommand(o => Save(o));
        }
        private string formula;
        public string Formula
        {
            get => formula;
            set
            {
                formula = value;
                FormulaText = $"y(x)={Formula}";
                OnPropertyChanged(nameof(Formula));
            }
        }
        private string formulaText = "y(x)=";
        public string FormulaText
        {
            get => formulaText;
            set
            {
                formulaText = value;
                OnPropertyChanged(nameof(FormulaText)); 
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
        private Color color;
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                OnPropertyChanged(nameof(Color));
            }
        }
        private string limitText;
        public string LimitText 
        { 
            get => limitText; 
            set
            {
                limitText = value;
                OnPropertyChanged(nameof(LimitText));
            }
        }
        public FormulaViewModel()
        {
            Name = DEFAULT_NAME;
            Description = DEFAULT_DESCRIPTION;
            Step = DEFAULT_STEP;
            LeftBound = DEFAULT_LEFT_BOUND;
            RightBound = DEFAULT_RIGHT_BOUND;
            LimitText = $"x\u2208[{LeftBound};{RightBound}]";
            Color = Brushes.Red.Color;
        }
        public FormulaViewModel(FormulaFunction formulaFunction)
        {
            Function = formulaFunction;
            Formula = formulaFunction.Formula;
            Step = formulaFunction.Step;
            Name = Function.Name;
            Description = Function.Description;
            LeftBound = Function.MinArg;
            RightBound = Function.MaxArg;
            Color = Function.Color;
        }
        private void EditColor()
        {
            Color color = new Color();
            ColorProcessor.EditColor(ref color);
            Color = color;
        }
        private void Save(object parameter)
        {
            if (Function is null)
            {
                Function = new FormulaFunction(isUser: true);
            }
            Function.Name = Name;
            Function.Description = Description;
            Function.IsUser = true;
            (Function as FormulaFunction).Step = Step;
            (Function as FormulaFunction).Formula = Formula;
            Function.Color = Color;
            Function.MinArg = LeftBound;
            Function.MaxArg = RightBound;
            var window = parameter as Window;
            if (LeftBound > RightBound)
            {
                MessageBox.Show($"{ERROR_BOUNDS}");
                return;
            }
            if (Step > MAX_STEP)
            {
                MessageBox.Show($"{ERROR_STEP} {MAX_STEP}");
                return;
            }
            window.DialogResult = true;
            window.Close();
        }

    }
}
