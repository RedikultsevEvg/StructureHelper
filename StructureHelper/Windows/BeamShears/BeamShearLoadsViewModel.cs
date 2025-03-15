using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearLoadsViewModel : SelectItemVM<IBeamShearLoad>
    {
        public BeamShearLoadsViewModel(List<IBeamShearLoad> collection) : base(collection)
        {
        }

        public override void EditMethod(object parameter)
        {
            if (SelectedItem is IDistributedLoad distributedLoad)
            {

            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
            base.EditMethod(parameter);
        }
    }
}
