using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceCombinationViewModel : ForceActionVMBase
    {
        ForceTuplesViewModel designForces;

        public ForceTuplesViewModel DesignForces => designForces;

        public ForceCombinationViewModel(IForceCombinationList combinationList) : base(combinationList)
        {
            designForces = new ForceTuplesViewModel(combinationList.DesignForces);
        }
    }
}
