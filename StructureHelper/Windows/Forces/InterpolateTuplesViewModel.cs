using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using System.Windows.Input;

namespace StructureHelper.Windows.Forces
{
    public class InterpolateTuplesViewModel : OkCancelViewModelBase
    {
        public ForceTupleInterpolationViewModel ForceInterpolationViewModel { get; set; }

        public InterpolateTuplesViewModel(IDesignForceTuple finishDesignForce, IDesignForceTuple startDesignForce=null, int stepCount = 100)
        {
            ForceInterpolationViewModel = new(finishDesignForce, startDesignForce, stepCount);
        }
    }
}
