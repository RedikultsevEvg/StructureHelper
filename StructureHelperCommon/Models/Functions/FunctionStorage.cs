using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Functions
{
    public class FunctionStorage
    {
        public IOneVariableFunction Func_ST_ULS { get; set; }
        public IOneVariableFunction Func_ST_SLS { get; set; }
        public IOneVariableFunction Func_ST_Special { get; set; }
        public IOneVariableFunction Func_LT_ULS { get; set; }
        public IOneVariableFunction Func_LT_SLS { get; set; }
        public IOneVariableFunction Func_LT_Special { get; set; }

    }
}
