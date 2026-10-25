using StructureHelper.Windows.CalculationWindows.CalculatorsViews;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StructureHelper.Windows.LoadHistory
{
    internal class HistoryCalculatorResultDiagramFactory
    {
        IGetUnitLogic unitLogic = new GetUnitLogic();
        private ArrayParameter<double> arrayParameter;

        public IHistoryCalculatorResult HistoryCalculatorResult
        {
            get;
            set
            {
                field = value;
                stagesResults = HistoryCalculatorResult.StagesResults.Where(x => x.IsValid).ToList();
                subStagesResults = [];
                foreach (var item in stagesResults)
                {
                    subStagesResults.AddRange(item.SubStagesResults.Where(x => x.IsValid));
                }
            }
        }
        private List<IStageResult> stagesResults;
        private List<ISubStageResult> subStagesResults;

        public ArrayParameter<double> GetCommonArray()
        {
            List<string> labels = GetLabels();
            arrayParameter = new ArrayParameter<double>(subStagesResults.Count, labels);
            Calculate();
            return arrayParameter;
        }

        private void Calculate()
        {
            var data = arrayParameter.Data;
            for (int subStageIterator = 0; subStageIterator < subStagesResults.Count; subStageIterator++)
            {
                var valueList = ProcessResult(subStageIterator);
                for (int j = 0; j < valueList.Count; j++)
                {
                    data[subStageIterator, j] = valueList[j];
                }
                //SetProgress?.Invoke(i);
            }
        }

        private List<double> ProcessResult(int i)
        {
            var unitForce = unitLogic.GetUnit(UnitTypes.Force);
            var unitMoment = unitLogic.GetUnit(UnitTypes.Moment);
            var unitCurvature = unitLogic.GetUnit(UnitTypes.Curvature);

            return new List<double>
                {
                    subStagesResults[i].ForcesTupleResult.ForceTuple.Mx * unitMoment.Multiplayer,
                    subStagesResults[i].ForcesTupleResult.ForceTuple.My * unitMoment.Multiplayer,
                    subStagesResults[i].ForcesTupleResult.ForceTuple.Nz * unitForce.Multiplayer,
                    subStagesResults[i].ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix.Kx * unitCurvature.Multiplayer,
                    subStagesResults[i].ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix.Ky * unitCurvature.Multiplayer,
                    subStagesResults[i].ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix.EpsZ,
                    subStagesResults[i].TotalTime,
                    subStagesResults[i].TotalNumber,
                };
        }

        private static List<string> GetLabels()
        {
            var labels = LabelsFactory.GetCommonLabels();
            var extralabels = new List<string>
            {
                $"Time",
                $"Step",
            };
            labels.AddRange(extralabels);
            return labels;
        }
    }
}
