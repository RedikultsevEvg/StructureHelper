using StructureHelper.Infrastructure;
using StructureHelperLogics.NdmCalculations.Cracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class CrackResultViewModel : ViewModelBase
    {
        CrackResult crackResult;
        public TupleCrackResult SelectedResult { get; set; }
        public List<TupleCrackResult> TupleResults => CrackResult.TupleResults;

        public CrackResult CrackResult => crackResult;

        public CrackResultViewModel(CrackResult crackResult)
        {
            this.crackResult = crackResult;
        }
    }
}
