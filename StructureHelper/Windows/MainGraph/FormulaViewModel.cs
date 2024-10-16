using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.MainGraph
{
    public class FormulaViewModel
    {
        private IOneVariableFunction function;
        public IOneVariableFunction Function
        {
            get => function;
            set
            {
                function = value;
            }
        }
        public FormulaViewModel() 
        {
            
        }
        public FormulaViewModel(FormulaFunction function)
        {
            
        }
    }
}
