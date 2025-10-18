using StructureHelper.Windows.ViewModels.Forces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace StructureHelper.Windows.Forces
{
    public class ForceCombinationByFactorVM : ForceActionVMBase, IDataErrorInfo
    {
        IForceFactoredList model;
        ForceTupleVM forceTupleVM;
        private FactoredCombinationPropertyVM combinationProperty;

        public ForceTupleVM ForceTupleVM => forceTupleVM;
        public FactoredCombinationPropertyVM CombinationProperty
        {
            get => combinationProperty;
            set
            {
                combinationProperty = value;
                OnPropertyChanged(nameof(CombinationProperty));
            }
        }


        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = null;
                return error;
            }
        }

        public ForceCombinationByFactorVM(IForceFactoredList model) : base(model)
        {
            this.model = model;
            forceTupleVM = new ForceTupleVM(this.model.ForceTuples[0]);
            CombinationProperty = new FactoredCombinationPropertyVM(model.CombinationProperty);
        }
    }
}
