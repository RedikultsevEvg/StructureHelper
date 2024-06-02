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
        private readonly List<ForceResultFunc> resultFuncs;
        public SelectItemsVM<ForceResultFunc> ResultFuncs { get; }
        public ValueDelegatesLogic()
        {
            resultFuncs = new List<ForceResultFunc>();
            resultFuncs.AddRange(ForceResultFuncFactory.GetResultFuncs(FuncsTypes.Full));
            ResultFuncs = new SelectItemsVM<ForceResultFunc>(resultFuncs)
            {
                ShowButtons = true
            };
            ResultFuncs.InvertSelection();
        }
    }
}
