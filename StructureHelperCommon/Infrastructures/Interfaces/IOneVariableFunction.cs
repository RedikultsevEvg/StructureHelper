using StructureHelperCommon.Infrastructures.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface IOneVariableFunction
    {
        public FunctionType Type { get; set; }
        public string Name { get; set; }
        public bool Check();
        public double GetByX(double xValue);
    }
}
