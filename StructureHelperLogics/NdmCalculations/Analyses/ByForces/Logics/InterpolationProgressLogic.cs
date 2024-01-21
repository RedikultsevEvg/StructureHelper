using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Services.NdmCalculations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public ITraceLogger? TraceLogger { get; set; }

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
