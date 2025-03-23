using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class FactoredForceTupleViewModel : ViewModelBase
    {
        private readonly IFactoredForceTuple factoredForceTuple;
        public ForceTupleVM ForceTuple { get; }
        public FactoredCombinationPropertyVM CombinationProperty { get; }

        public FactoredForceTupleViewModel(IFactoredForceTuple factoredForceTuple)
        {
            this.factoredForceTuple = factoredForceTuple;
            ForceTuple = new(this.factoredForceTuple.ForceTuple);
            CombinationProperty = new(this.factoredForceTuple.CombinationProperty);
        }
    }
}
