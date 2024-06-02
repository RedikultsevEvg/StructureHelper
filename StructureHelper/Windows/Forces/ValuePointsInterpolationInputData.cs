using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.Forces
{
    public class ValuePointsInterpolationInputData
    {
        public IDesignForceTuple FinishDesignForce { get; set; }
        public IDesignForceTuple StartDesignForce { get; set; }
        public int StepCount { get; set; }
        public List<PrimitiveBase> PrimitiveBases { get; private set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public ValuePointsInterpolationInputData()
        {
            PrimitiveBases = new List<PrimitiveBase>();
            StepCount = 100;
        }

    }
}
