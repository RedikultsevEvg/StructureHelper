using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace StructureHelper.Services.Exports
{
    /// <summary>
    /// Builds a tabular representation of load history results for export and clipboard operations.
    /// Force values are given in kN, moments in kNm and curvatures in 1/mm.
    /// </summary>
    public static class HistoryResultTableBuilder
    {
        private static readonly IUnit kilonewton = new GetUnitLogic().GetUnit(UnitTypes.Force, "kN");
        private static readonly IUnit kilonewtonMeter = new GetUnitLogic().GetUnit(UnitTypes.Moment, "kNm");
        private static readonly IUnit perMillimeter = new GetUnitLogic().GetUnit(UnitTypes.Curvature, "1/mm");

        public static IReadOnlyList<string> Headers { get; } = new[]
        {
            "Stage",
            "Valid",
            "Position",
            "Step",
            "Time, s",
            "Primitives",
            "Nz, kN",
            "Mx, kNm",
            "My, kNm",
            "EpsZ",
            "Kx, 1/mm",
            "Ky, 1/mm",
            "Accuracy",
            "Iterations",
            "Description"
        };

        public static List<object?[]> GetRows(IEnumerable<ISubStageResult> results)
        {
            var rows = new List<object?[]>();
            foreach (ISubStageResult result in results)
            {
                var loaderResults = result.ForcesTupleResult?.LoaderResults;
                var forceStrainPair = loaderResults?.ForceStrainPair;
                var strainMatrix = forceStrainPair?.StrainMatrix;

                double nz = result.ForceTuple.Nz;
                double mx = result.ForceTuple.Mx;
                double my = result.ForceTuple.My;
                double epsZ = strainMatrix?.EpsZ ?? result.StrainTuple.Nz;
                double kx = strainMatrix?.Kx ?? result.StrainTuple.Mx;
                double ky = strainMatrix?.Ky ?? result.StrainTuple.My;

                rows.Add(new object?[]
                {
                    result.ConstructionStage.Name,
                    result.IsValid ? "TRUE" : "FALSE",
                    result.StagePosition.ToString(),
                    result.TotalNumber,
                    result.TotalTime,
                    result.Primitives.Count,
                    nz * kilonewton.Multiplayer,
                    mx * kilonewtonMeter.Multiplayer,
                    my * kilonewtonMeter.Multiplayer,
                    epsZ,
                    kx * perMillimeter.Multiplayer,
                    ky * perMillimeter.Multiplayer,
                    loaderResults?.AccuracyRate,
                    loaderResults?.IterationCounter,
                    result.Description ?? string.Empty
                });
            }
            return rows;
        }

        public static string ToTabSeparatedText(IEnumerable<ISubStageResult> results)
        {
            var builder = new StringBuilder();
            builder.AppendLine(string.Join('\t', Headers));
            foreach (object?[] row in GetRows(results))
            {
                builder.AppendLine(string.Join('\t', row.Select(FormatCell)));
            }
            return builder.ToString();
        }

        private static string FormatCell(object? value) => value switch
        {
            null => string.Empty,
            double doubleValue => doubleValue.ToString("G17", CultureInfo.InvariantCulture),
            int intValue => intValue.ToString(CultureInfo.InvariantCulture),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty
        };
    }
}
