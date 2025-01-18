using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ForceCombinationViewerVM : ViewModelBase
    {
        private IForceAction model;

        public List<IForceCombinationList> Combinations => model.GetCombinations();

        public ForceCombinationViewerVM(IForceAction model)
        {
            this.model = model;
        }
    }
}
