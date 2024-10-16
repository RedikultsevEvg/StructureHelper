using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace StructureHelperCommon.Models.Functions
{
    public class FormulaFunction : IOneVariableFunction
    {
        public bool IsUser { get; set; }
        public FunctionType Type { get; set; }
        public string Name { get; set; }
        public string Description { get ; set; }
        public string Formula {  get; set; }

        public Guid Id => throw new NotImplementedException();

        public bool Check()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            var formulaFunction = new FormulaFunction();

            //Здесь будет стратегия
            formulaFunction.Type = Type;
            formulaFunction.Name = Name;
            formulaFunction.Description = Description;
            formulaFunction.Formula = Formula;
            formulaFunction.IsUser = true;

            return formulaFunction;
        }

        public double GetByX(double xValue)
        {
            throw new NotImplementedException();
        }
    }
}
