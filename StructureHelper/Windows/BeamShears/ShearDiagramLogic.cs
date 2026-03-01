using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.BeamShears;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.BeamShears
{
    public class ShearDiagramLogic
    {
        private const string ForceUnitString = "kN";
        private IBeamShearActionResult result;
        private IUnit unitForce;

        public ShearDiagramLogic(IBeamShearActionResult result)
        {
            this.result = result;
            IGetUnitLogic unitLogic = new GetUnitLogic();
            unitForce = unitLogic.GetUnit(UnitTypes.Force, ForceUnitString);
        }

        public void ShowWindow(double sectionsStartCoordinate)
        {
            SafetyProcessor.RunSafeProcess(() =>
            {
                var seriesList = new List<Series>();
                var series = new Series(GetParametersByCurveResult(sectionsStartCoordinate)) { Name = "" };
                seriesList.Add(series);
                var vm = new GraphViewModel(seriesList);
                var wnd = new GraphView(vm);
                wnd.ShowDialog();
            },
            "Errors appeared during showing a chart, see detailed information");
        }
        private ArrayParameter<double> GetParametersByCurveResult(double sectionsStartCoordinate)
        {
            List<IBeamShearSectionLogicResult> results = result.SectionResults
                .Where(x => x.InputData.InclinedSection.StartCoord == sectionsStartCoordinate)
                .ToList();
            var labels = GetLabels();
            var arrayParameter = new ArrayParameter<double>(results.Count(), labels.Count(), labels);
            var data = arrayParameter.Data;
            for (int i = 0; i < results.Count(); i++)
            {
                var valueList = new List<double>
                    {
                    results[i].InputData.InclinedSection.EndCoord,
                    results[i].InputData.InclinedSection.EndCoord / results[i].InputData.InclinedSection.EffectiveDepth,
                    results[i].InputData.ForceTuple.Nz * unitForce.Multiplayer,
                    results[i].InputData.ForceTuple.Qy * unitForce.Multiplayer,
                    results[i].TotalStrength * unitForce.Multiplayer,
                    results[i].ConcreteStrength * unitForce.Multiplayer,
                    results[i].StirrupStrength * unitForce.Multiplayer,
                    results[i].FactorOfUsing,
                    };
                for (int j = 0; j < valueList.Count; j++)
                {
                    data[i, j] = valueList[j];
                }
            }
            return arrayParameter;
        }
        private List<string> GetLabels()
        {
            List<string> strings = new()
            {
                "End coord",
                "a/d-ratio",
                "Nz",
                "Qy",
                "Qult",
                "Qb",
                "Qsw",
                "Uf"
            };
            return strings;
        }
    }
}
