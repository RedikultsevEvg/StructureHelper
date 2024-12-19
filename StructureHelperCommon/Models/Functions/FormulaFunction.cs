using LiveCharts;
using Microsoft.VisualBasic.ApplicationServices;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using StructureHelperCommon.Services;
using LiveCharts.Wpf;
using StructureHelperCommon.Services.ColorServices;
using FunctionParser;

namespace StructureHelperCommon.Models.Functions
{
    public class FormulaFunction : IOneVariableFunction
    {
        private double current_xValue;
        private string formula;
        private Expression expression;
        private const string COPY = "copy";
        public const string GROUP_TYPE_1 = "System function";
        public const string GROUP_TYPE_2 = "User function";
        public bool IsUser { get; set; }
        public FunctionType Type { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get ; set; }
        public int Step { get; set; }
        public string Formula 
        {
            get
            {
                return formula;
            }
            set
            {
                formula = value;
                Expression = new Expression(value, new string[] { "x" }, null);
            }
        }
        public Expression Expression 
        { 
            get
            {
                return expression;
            }                 
            set
            {
                expression = value;
            }
        }
        public Guid Id => throw new NotImplementedException();
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = new ObservableCollection<IOneVariableFunction>();
        public double MinArg { get; set; }
        public double MaxArg { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public Color Color { get; set; }
        public string Trace { get; set; }

        public FormulaFunction(bool isUser = false)
        {
            Color = ColorProcessor.GetRandomColor();
            Type = FunctionType.FormulaFunction;
            if (isUser)
            {
                IsUser = true;
                Group = GROUP_TYPE_2;
            }
            else
            {
                IsUser = false;
                Group = GROUP_TYPE_1;
            }
        }

        public bool Check()
        {
            throw new NotImplementedException();
        }
        public object Clone()
        {
            var formulaFunction = new FormulaFunction();

            //Здесь будет стратегия
            formulaFunction.Type = Type;
            formulaFunction.Name = $"{Name} {COPY}"; 
            formulaFunction.Description = Description;
            formulaFunction.Formula = Formula;
            formulaFunction.IsUser = true;
            formulaFunction.Group = GROUP_TYPE_2;
            formulaFunction.Step = Step;
            formulaFunction.MinArg = MinArg;
            formulaFunction.MaxArg = MaxArg;
            return formulaFunction;
        }

        public double GetByX(double xValue)
        {
            double yValue = 0;
            current_xValue = xValue;
            yValue = Math.Round(Expression.CalculateValue(new double[] { xValue }), 2);
            return yValue;
        }
        public GraphSettings GetGraphSettings()
        {
            var graphSettings = new GraphSettings(Color, Color);
            var stepLenght = Math.Abs(MaxArg - MinArg) / Step;
            for (double x = MinArg; x < MaxArg; x += stepLenght)
            {
                var graphPoint = new GraphPoint(x, GetByX(x));
                graphSettings.GraphPoints.Add(graphPoint);
            }
            return graphSettings;
        }
    }
}
