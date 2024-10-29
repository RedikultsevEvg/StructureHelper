using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Functions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public abstract class FunctionDecorator : IOneVariableFunction
    {
        protected IOneVariableFunction function;
        public bool IsUser { get; set; }
        public string Group { get; set; }
        public FunctionType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<GraphPoint> Table { get; set; }
        public double MinArg { get; set; }
        public double MaxArg { get; set; }
        public ObservableCollection<IOneVariableFunction> Functions { get; set; } = new ObservableCollection<IOneVariableFunction>();

        public Guid Id => throw new NotImplementedException();

        public IShiftTraceLogger? TraceLogger { get; set; }
        public FunctionDecorator(IOneVariableFunction function)
        {
            this.function = function;
        }
        public virtual bool Check()
        {
            return function.Check();
        }
        public virtual object Clone()
        {
            return function.Clone();
        }
        public virtual double GetByX(double xValue)
        {
            return function.GetByX(xValue);
        }
    }
}
