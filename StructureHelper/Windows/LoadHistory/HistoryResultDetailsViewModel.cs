using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.LoadHistory;
using System;

namespace StructureHelper.Windows.LoadHistory
{
    /// <summary>
    /// Read-only details of one load history step: achieved accuracy, iterations, forces and curvatures.
    /// </summary>
    public class HistoryResultDetailsViewModel
    {
        private static readonly IUnit kilonewton = new GetUnitLogic().GetUnit(UnitTypes.Force, "kN");
        private static readonly IUnit kilonewtonMeter = new GetUnitLogic().GetUnit(UnitTypes.Moment, "kNm");
        private static readonly IUnit perMillimeter = new GetUnitLogic().GetUnit(UnitTypes.Curvature, "1/mm");

        private readonly ISubStageResult result;

        public HistoryResultDetailsViewModel(ISubStageResult result)
        {
            this.result = result ?? throw new ArgumentNullException(nameof(result));

            var loaderResults = result.ForcesTupleResult?.LoaderResults;
            var forceStrainPair = loaderResults?.ForceStrainPair;
            var forceMatrix = forceStrainPair?.ForceMatrix;
            var strainMatrix = forceStrainPair?.StrainMatrix;

            AchievedAccuracy = loaderResults?.AccuracyRate;
            IterationCount = loaderResults?.IterationCounter;

            var accuracy = result.ForcesTupleResult?.InputData?.Accuracy;
            RequiredAccuracy = accuracy?.IterationAccuracy;
            MaxIterationCount = accuracy?.MaxIterationCount;

            double nz = forceMatrix?.Nz ?? result.ForceTuple.Nz;
            double mx = forceMatrix?.Mx ?? result.ForceTuple.Mx;
            double my = forceMatrix?.My ?? result.ForceTuple.My;
            double epsZ = strainMatrix?.EpsZ ?? result.StrainTuple.Nz;
            double kx = strainMatrix?.Kx ?? result.StrainTuple.Mx;
            double ky = strainMatrix?.Ky ?? result.StrainTuple.My;

            NzKilonewtons = nz * kilonewton.Multiplayer;
            MxKilonewtonMeters = mx * kilonewtonMeter.Multiplayer;
            MyKilonewtonMeters = my * kilonewtonMeter.Multiplayer;
            EpsZ = epsZ;
            KxPerMillimeter = kx * perMillimeter.Multiplayer;
            KyPerMillimeter = ky * perMillimeter.Multiplayer;
        }

        public string StageName => result.ConstructionStage.Name;

        public string Position => result.StagePosition.ToString();

        public int StepNumber => result.TotalNumber;

        public double Time => result.TotalTime;

        public int PrimitivesCount => result.Primitives.Count;

        public bool IsValid => result.IsValid;

        public string Description => result.Description ?? string.Empty;

        public double? AchievedAccuracy { get; }

        public double? RequiredAccuracy { get; }

        public int? IterationCount { get; }

        public int? MaxIterationCount { get; }

        public double NzKilonewtons { get; }

        public double MxKilonewtonMeters { get; }

        public double MyKilonewtonMeters { get; }

        public double EpsZ { get; }

        public double KxPerMillimeter { get; }

        public double KyPerMillimeter { get; }
    }
}
