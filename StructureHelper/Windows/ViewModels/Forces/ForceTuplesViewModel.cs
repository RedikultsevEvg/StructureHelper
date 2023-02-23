using StructureHelper.Properties;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceTuplesViewModel : CRUDViewModelBase<IDesignForceTuple>
    {
        public override void AddMethod(object parameter)
        {
            NewItem = new DesignForceTuple() { LimitState=LimitStates.ULS, CalcTerm=CalcTerms.ShortTerm};
            base.AddMethod(parameter);
        }

        public ForceTuplesViewModel(List<IDesignForceTuple> collection) : base(collection)
        {
            
        }
    }
}
