using Microsoft.VisualBasic.ApplicationServices;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace StructureHelperCommon.Models.Functions
{
    public class FormulaFunction : IOneVariableFunction
    {
        private const string COPY = "copy";
        public const string GROUP_TYPE_1 = "System function";
        public const string GROUP_TYPE_2 = "User function";
        public bool IsUser { get; set; }
        public FunctionType Type { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public string Description { get ; set; }
        public List<GraphPoint> Table { get; set; }
        public string Formula {  get; set; }

        public Guid Id => throw new NotImplementedException();

        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = new ObservableCollection<IOneVariableFunction>();
        public FormulaFunction(bool isUser = false)
        {
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
            return formulaFunction;
        }

        public double GetByX(double xValue)
        {
            throw new NotImplementedException();
        }
    }
}
