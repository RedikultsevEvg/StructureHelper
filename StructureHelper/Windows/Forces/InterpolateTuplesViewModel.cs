using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;

namespace StructureHelper.Windows.Forces
{
    public class InterpolateTuplesViewModel : OkCancelViewModelBase
    {
        public ForceTupleInterpolationViewModel ForceInterpolationViewModel { get; set; }

        public InterpolateTuplesViewModel(IForceTuple startTuple, IForceTuple endTuple, int stepCount = 100)
        {
            ForceInterpolationViewModel = new(startTuple, endTuple, stepCount);
        }
    }
}
