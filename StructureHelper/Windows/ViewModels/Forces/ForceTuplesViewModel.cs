using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System.Collections.Generic;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceTuplesViewModel : SelectItemVM<IDesignForceTuple>
    {
        public override void AddMethod(object parameter)
        {
            NewItem = new DesignForceTuple() { LimitState=LimitStates.ULS, CalcTerm=CalcTerms.ShortTerm};
            base.AddMethod(parameter);
        }

        public override void CopyMethod(object parameter)
        {
            AutoCopy();
            SelectedItem = NewItem;
        }

        public ForceTuplesViewModel(List<IDesignForceTuple> collection) : base(collection)
        {
            
        }
    }
}
