using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions
{
    public class FormulaFunction : IOneVariableFunction
    {
        public FunctionType Type { get; set; }
        public string Name { get; set; }

        public bool Check()
        {
            throw new NotImplementedException();
        }
        public double GetByX(double xValue)
        {
            throw new NotImplementedException();
        }
    }

    public class CopyOfFormulaFunction : IOneVariableFunction
    {
        public FunctionType Type { get; set; }
        public string Name { get; set; }

        public bool Check()
        {
            throw new NotImplementedException();
        }
        public double GetByX(double xValue)
        {
            throw new NotImplementedException();
        }
    }
}
