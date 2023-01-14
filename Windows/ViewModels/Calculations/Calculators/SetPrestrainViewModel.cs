using FieldVisualizer.ViewModels;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    public class SetPrestrainViewModel : ViewModelBase
    {
        IStrainTuple SourceTuple;
        private double coeffcient;

        public double Coefficient
        {
            get
            {
                return coeffcient;
            }
            set
            {
                SetProperty(ref coeffcient, value);
            }
        }

        public SetPrestrainViewModel(IStrainTuple sourceTuple)
        {
            SourceTuple = sourceTuple;
            coeffcient = 1d;
        }

        public IStrainTuple GetStrainTuple()
        {
            var result = new StrainTuple();
            StrainTupleService.CopyProperties(SourceTuple, result, coeffcient);
            return result;
        }
    }
}
