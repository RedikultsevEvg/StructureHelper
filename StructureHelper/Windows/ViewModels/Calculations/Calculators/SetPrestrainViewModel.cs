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
        StrainTuple SourceTuple;
        private double coefficient;

        public double Coefficient
        {
            get
            {
                return coefficient;
            }
            set
            {
                SetProperty(ref coefficient, value);
            }
        }

        public SetPrestrainViewModel(StrainTuple sourceTuple)
        {
            SourceTuple = sourceTuple;
            coefficient = 1d;
        }

        public StrainTuple GetStrainTuple()
        {
            var result = new StrainTuple();
            ForceTupleService.CopyProperties(SourceTuple, result, coefficient);
            return result;
        }
    }
}
