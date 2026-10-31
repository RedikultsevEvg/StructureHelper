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
        private readonly List<ResultFuncByStrainMatrix> resultFuncs;
        public SelectItemsVM<ResultFuncByStrainMatrix> ResultFuncs { get; }
        public ValueDelegatesLogic()
        {
            resultFuncs = new List<ResultFuncByStrainMatrix>();
            resultFuncs.AddRange(NdmStressStrainResultFuncFactory.GetResultFuncs(FuncsTypes.Full));
            ResultFuncs = new SelectItemsVM<ResultFuncByStrainMatrix>(resultFuncs)
            {
                ShowButtons = true
            };
            ResultFuncs.InvertSelection();
        }
    }
}
