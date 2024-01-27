using LoaderCalculator;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    internal class CrackDiagramLogic : ILongProcessLogic
    {
        static readonly CrackForceCalculator calculator = new();
        private List<IForcesTupleResult> ValidTupleList { get; set; }
        ArrayParameter<double> arrayParameter;
        private IEnumerable<IForcesTupleResult> TupleList { get; set; }
        private IEnumerable<INdmPrimitive> NdmPrimitives { get; set; }

        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public Action<int> SetProgress { get; set; }
        public bool Result { get; set; }

        public int StepCount => ValidTupleList.Count();

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackDiagramLogic(IEnumerable<IForcesTupleResult> tupleList, IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            TupleList = tupleList;
            NdmPrimitives = ndmPrimitives;
            ValidTupleList = TupleList.Where(x => x.IsValid == true).ToList();
        }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            ShowCracks();
            Result = true;
        }

        public void WorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //nothing to do
        }

        public void WorkerRunWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //nothing to do
        }

        public void ShowCracks()
        {
            var unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
            var unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
            var unitCurvature = CommonOperation.GetUnit(UnitTypes.Curvature, "1/m");

            string[] labels = GetCrackLabels(unitForce, unitMoment, unitCurvature);
            arrayParameter = new ArrayParameter<double>(ValidTupleList.Count(), labels.Count(), labels);
            CalculateWithCrack(ValidTupleList, NdmPrimitives, unitForce, unitMoment, unitCurvature);
        }

        public void ShowWindow()
        {
            SafetyProcessor.RunSafeProcess(() =>
            {
                var series = new Series(arrayParameter) { Name = "Forces and curvatures" };
                var vm = new GraphViewModel(new List<Series>() { series });
                var wnd = new GraphView(vm);
                wnd.ShowDialog();
            },
            "Errors appeared during showing a graph, see detailed information");
        }

        private void CalculateWithCrack(List<IForcesTupleResult> validTupleList, IEnumerable<INdmPrimitive> ndmPrimitives, IUnit unitForce, IUnit unitMoment, IUnit unitCurvature)
        {
            var data = arrayParameter.Data;
            for (int i = 0; i < validTupleList.Count(); i++)
            {
                var valueList = new List<double>
                {
                    validTupleList[i].DesignForceTuple.ForceTuple.Mx * unitMoment.Multiplyer,
                    validTupleList[i].DesignForceTuple.ForceTuple.My * unitMoment.Multiplyer,
                    validTupleList[i].DesignForceTuple.ForceTuple.Nz * unitForce.Multiplyer
                };
                calculator.EndTuple = validTupleList[i].DesignForceTuple.ForceTuple;
                var limitState = validTupleList[i].DesignForceTuple.LimitState;
                var calcTerm = validTupleList[i].DesignForceTuple.CalcTerm;
                var ndms = NdmPrimitivesService.GetNdms(ndmPrimitives, limitState, calcTerm);
                calculator.NdmCollection = ndms;
                calculator.Run();
                var result = (CrackForceResult)calculator.Result;
                if (result.IsValid == false)
                {
                    MessageBox.Show(
                        "Result is not valid",
                        "Crack results",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                valueList.Add(result.CrackedStrainTuple.Mx);
                valueList.Add(result.CrackedStrainTuple.My);
                valueList.Add(result.CrackedStrainTuple.Nz);

                valueList.Add(result.ReducedStrainTuple.Mx);
                valueList.Add(result.ReducedStrainTuple.My);
                valueList.Add(result.ReducedStrainTuple.Nz);

                valueList.Add(result.SofteningFactors.Mx);
                valueList.Add(result.SofteningFactors.My);
                valueList.Add(result.SofteningFactors.Nz);

                valueList.Add(result.PsiS);

                for (int j = 0; j < valueList.Count; j++)
                {
                    data[i, j] = valueList[j];
                }

                SetProgress?.Invoke(i);
            }
        }

        private static string[] GetCrackLabels(IUnit unitForce, IUnit unitMoment, IUnit unitCurvature)
        {
            const string crc = "Crc";
            const string crcFactor = "CrcSofteningFactor";
            return new string[]
            {
                $"{GeometryNames.MomFstName}, {unitMoment.Name}",
                $"{GeometryNames.MomSndName}, {unitMoment.Name}",
                $"{GeometryNames.LongForceName}, {unitForce.Name}",
                $"{GeometryNames.CurvFstName}, {unitCurvature.Name}",
                $"{GeometryNames.CurvSndName}, {unitCurvature.Name}",
                $"{GeometryNames.StrainTrdName}",
                $"{crc}{GeometryNames.CurvFstName}, {unitCurvature.Name}",
                $"{crc}{GeometryNames.CurvSndName}, {unitCurvature.Name}",
                $"{crc}{GeometryNames.StrainTrdName}",
                $"{crcFactor}Ix",
                $"{crcFactor}Iy",
                $"{crcFactor}Az",
                $"PsiFactor"
            };
        }

    }
}
