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
        private string name;

        public string Name
        {
            get => shearAction.Name;
            set
            {
                shearAction.Name = value;
            }
        }
        public BeamShearAxisActionViewModel YAxisAction { get; private set; }

        public BeamShearActionViewModel(IBeamShearAction shearAction)
        {
            this.shearAction = shearAction;
            YAxisAction = new(this.shearAction.YAxisShearAction);
        }
    }
}
