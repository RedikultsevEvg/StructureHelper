using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.TreeGraph
{
    public static class Database
    {
        public static IOneVariableFunction GetFunctionTree()
        {
            return new TableFunction()
            {
                Name = "func0",
                Functions = new ObservableCollection<IOneVariableFunction>()
                {
                    new FormulaFunction()
                    {
                        Name = "func1.1"
                    },
                    new FormulaFunction()
                    {
                        Name = "func1.2"
                    },
                    new FormulaFunction()
                    {
                        Name = "func1.3",
                        Functions = new ObservableCollection<IOneVariableFunction>()
                        {
                            new FormulaFunction()
                            {
                                Name = "func2.1"
                            },
                            new FormulaFunction()
                            {
                                Name = "func2.2"
                            },
                        }
                    },
                    new TableFunction()
                    {
                        Name = "func1.4"
                    }
                }
            };
        }
    }
}
