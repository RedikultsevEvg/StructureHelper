using StructureHelper.Infrastructure;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ValueDelegatesLogic : ViewModelBase
    {
        private readonly List<IResultFunc> resultFuncs;
        public SelectItemsVM<IResultFunc> ResultFuncs { get; }
        public ValueDelegatesLogic()
        {
            resultFuncs = new List<IResultFunc>();
            resultFuncs.AddRange(ResultFuncFactory.GetResultFuncs(FuncsTypes.Strain));
            resultFuncs.AddRange(ResultFuncFactory.GetResultFuncs(FuncsTypes.Stress));
            ResultFuncs = new SelectItemsVM<IResultFunc>(resultFuncs)
            {
                ShowButtons = true
            };
            ResultFuncs.InvertSelection();
        }
    }
}
