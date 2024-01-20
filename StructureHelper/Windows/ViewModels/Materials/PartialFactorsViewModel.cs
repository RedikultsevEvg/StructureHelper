using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class PartialFactorsViewModel : SelectItemVM<IMaterialPartialFactor>
    {
        public override void AddMethod(object parameter)
        {
            NewItem = new MaterialPartialFactor();
            base.AddMethod(parameter);
        }

        public PartialFactorsViewModel(List<IMaterialPartialFactor> safetyFactors) : base(safetyFactors)
        {
        }
    }
}
