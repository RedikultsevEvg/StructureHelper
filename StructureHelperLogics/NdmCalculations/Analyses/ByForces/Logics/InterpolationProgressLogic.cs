using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.Services.NdmCalculations;
using System.ComponentModel;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    public class InterpolationProgressLogic : ILongProcessLogic
    {
        private IForceCalculator forceCalculator;
        private IStateCalcTermPair stateCalcTermPair;
        private InterpolateTuplesResult interpolateTuplesResult;

        public Action<int> SetProgress { get; set; }

        public IForceCalculator InterpolateCalculator { get; private set; }
        public bool Result { get; set; }

        public int StepCount => interpolateTuplesResult.StepCount + 1;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            InterpolateCalculator = InterpolateService.InterpolateForceCalculator(forceCalculator, stateCalcTermPair, interpolateTuplesResult);
            InterpolateCalculator.ActionToOutputResults = ShowProgressResult;
            InterpolateCalculator.Run();
        }

        public void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //nothing to do
        }

        public void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //nothing to do
        }

        public InterpolationProgressLogic(IForceCalculator forceCalculator, IStateCalcTermPair stateCalcTermPair, InterpolateTuplesResult interpolateTuplesResult)
        {
            this.forceCalculator = forceCalculator;
            this.stateCalcTermPair = stateCalcTermPair;
            this.interpolateTuplesResult = interpolateTuplesResult;
        }

        private void ShowProgressResult(IResult result)
        {
            if (result is ForceCalculatorResult)
            {
                var forceResult = result as ForceCalculatorResult;
                SetProgress?.Invoke(forceResult.ForcesResultList.Count());
                Result = forceResult.IsValid;
            }
        }
    }
}
