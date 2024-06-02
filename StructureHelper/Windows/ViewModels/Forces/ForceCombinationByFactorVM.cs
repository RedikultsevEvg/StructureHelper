using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceCombinationByFactorVM : ForceActionVMBase, IDataErrorInfo
    {
        IForceCombinationByFactor forceAction;
        ForceTupleVM forceTupleVM;

        public double ULSFactor
        {
            get => forceAction.ULSFactor;
            set
            {
                forceAction.ULSFactor = value;
                OnPropertyChanged(nameof(ULSFactor));
            }
        }

        public ForceTupleVM ForceTupleVM => forceTupleVM;

        public double LongTermFactor
        {
            get => forceAction.LongTermFactor;
            set
            {
                if (value <0d) { value = 0d; }
                if (value > 1d) { value = 1d; }
                forceAction.LongTermFactor = value;
                OnPropertyChanged(nameof(LongTermFactor));
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == nameof(ULSFactor))
                {
                    if (ULSFactor <= 0)
                    {
                        error = "Safety factor for ULS must be greater than zero";
                    }
                }
                if (columnName == nameof(LongTermFactor))
                {
                    if (LongTermFactor < 0d || LongTermFactor > 1d)
                    {
                        error = "Long term factor must be between 0.0 and 1.0";
                    }
                }
                return error;
            }
        }

        public ForceCombinationByFactorVM(IForceCombinationByFactor forceAction) : base(forceAction)
        {
            this.forceAction = forceAction;
            forceTupleVM = new ForceTupleVM(forceAction.FullSLSForces);
        }
    }
}
