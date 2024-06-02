using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ValuePointsInterpolateViewModel : OkCancelViewModelBase
    {
        private readonly ValuePointsInterpolationInputData inputData;

        public ForceTupleInterpolationViewModel ForceInterpolationViewModel { get; private set; }
        public PointPrimitiveLogic PrimitiveLogic { get; private set; }
        public ValueDelegatesLogic ValueDelegatesLogic { get; set; }
        public ValuePointsInterpolateViewModel(ValuePointsInterpolationInputData inputData)
        {
            this.inputData = inputData;
            ForceInterpolationViewModel = new(this.inputData.FinishDesignForce, this.inputData.StartDesignForce, this.inputData.StepCount);
            PrimitiveLogic = new PointPrimitiveLogic(inputData.PrimitiveBases);
            ValueDelegatesLogic = new();
        }
    }
}
