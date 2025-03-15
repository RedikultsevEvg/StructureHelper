using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearActionViewModel : OkCancelViewModelBase
    {
        private readonly IBeamShearAction shearAction;
        public BeamShearLoadsViewModel ShearLoads { get; private set; }

        public BeamShearActionViewModel(IBeamShearAction shearAction)
        {
            this.shearAction = shearAction;
            ShearLoads = new(this.shearAction.YAxisShearAction.ShearLoads);
        }
    }
}
