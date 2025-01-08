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
        IForceFactoredList forceAction;
        ForceTupleVM forceTupleVM;

        public ForceTupleVM ForceTupleVM => forceTupleVM;
        public IFactoredCombinationProperty CombinationProperty
        {
            get
            {
                return forceAction.CombinationProperty;
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = null;

                //if (columnName == nameof(ULSFactor))
                //{
                //    if (ULSFactor <= 0)
                //    {
                //        error = "Safety factor for ULS must be greater than zero";
                //    }
                //}
                //if (columnName == nameof(LongTermFactor))
                //{
                //    if (LongTermFactor < 0d || LongTermFactor > 1d)
                //    {
                //        error = "Long term factor must be between 0.0 and 1.0";
                //    }
                //}
                return error;
            }
        }

        public ForceCombinationByFactorVM(IForceFactoredList forceAction) : base(forceAction)
        {
            this.forceAction = forceAction;
            forceTupleVM = new ForceTupleVM(this.forceAction.ForceTuples[0]);
        }
    }
}
