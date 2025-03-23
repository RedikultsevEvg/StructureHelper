using StructureHelper.Windows.Forces;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Forces;
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
        private string name;

        public string Name
        {
            get => shearAction.Name;
            set
            {
                shearAction.Name = value;
            }
        }

        public FactoredForceTupleViewModel ExternalForces { get; private set; }

        public BeamShearAxisActionViewModel SupportAction { get; private set; }

        public BeamShearActionViewModel(IBeamShearAction shearAction)
        {
            this.shearAction = shearAction;
            SupportAction = new(this.shearAction.SupportAction);
            ExternalForces = new(this.shearAction.ExternalForce);
        }
    }
}
