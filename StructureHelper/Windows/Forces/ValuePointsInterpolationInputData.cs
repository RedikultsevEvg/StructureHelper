using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using System.Collections.Generic;

namespace StructureHelper.Windows.Forces
{
    public class ValuePointsInterpolationInputData
    {
        public IForceTuple FinishForceTuple { get; set; }
        public IForceTuple StartForceTuple { get; set; }
        public int StepCount { get; set; }
        public List<PrimitiveBase> PrimitiveBases { get; private set; }
        public IStateCalcTermPair StateCalcTermPair { get; set; }

        public ValuePointsInterpolationInputData()
        {
            PrimitiveBases = new List<PrimitiveBase>();
            StepCount = 100;
        }

    }
}
