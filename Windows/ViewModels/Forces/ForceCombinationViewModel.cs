using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceCombinationViewModel : ViewModelBase
    {
        IForceCombinationList combinationList;

        public IDesignForceTuple SelectedTuple { get; set; }
        public string Name
        {
            get => combinationList.Name;
            set
            {
                combinationList.Name = value;
            }
        }
        public IEnumerable<IDesignForceTuple>  ForceTuples { get => combinationList.DesignForces; }

        public ForceCombinationViewModel(IForceCombinationList combinationList)
        {
            this.combinationList = combinationList;
        }
    }
}
