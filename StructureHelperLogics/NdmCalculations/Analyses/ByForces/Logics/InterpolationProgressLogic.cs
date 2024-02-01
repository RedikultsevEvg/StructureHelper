using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Services.NdmCalculations;
using System.ComponentModel;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics
{
    public class InterpolationProgressLogic : ILongProcessLogic
    {
        private ForceCalculator forceCalculator;
        private InterpolateTuplesResult interpolateTuplesResult;

        public Action<int> SetProgress { get; set; }

        public ForceCalculator InterpolateCalculator { get; private set; }
        public bool Result { get; set; }

        public int StepCount => interpolateTuplesResult.StepCount + 1;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            InterpolateCalculator = InterpolateService.InterpolateForceCalculator(forceCalculator, interpolateTuplesResult);
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

        public InterpolationProgressLogic(ForceCalculator forceCalculator, InterpolateTuplesResult interpolateTuplesResult)
        {
            this.forceCalculator = forceCalculator;
            this.interpolateTuplesResult = interpolateTuplesResult;
        }

        private void ShowProgressResult(IResult result)
        {
            if (result is ForcesResults)
            {
                var forceResult = result as ForcesResults;
                SetProgress?.Invoke(forceResult.ForcesResultList.Count());
                Result = forceResult.IsValid;
            }
        }
    }
}
