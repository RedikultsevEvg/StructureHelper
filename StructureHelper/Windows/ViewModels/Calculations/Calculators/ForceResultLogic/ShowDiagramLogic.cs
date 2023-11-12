using LoaderCalculator;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.Graphs;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Infrastructures.Settings;
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

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    internal class ShowDiagramLogic : ILongProcessLogic
    {
        ArrayParameter<double> arrayParameter;
        private IEnumerable<IForcesTupleResult> TupleList;
        private IEnumerable<INdmPrimitive> NdmPrimitives;
        private List<IForcesTupleResult> ValidTupleList;

        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public int StepCount => ValidTupleList.Count();

        public Action<int> SetProgress { get ; set; }
        public bool Result { get; set; }

        public void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Show();
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
        public void ShowWindow()
        {
            SafetyProcessor.RunSafeProcess(() =>
            {
                var wnd = new GraphView(arrayParameter);
                wnd.ShowDialog();
            },
            "Errors appeared during showing a graph, see detailed information");
        }
        private void Show()
        {
            ValidTupleList = TupleList.Where(x => x.IsValid == true).ToList();
            var unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
            var unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
            var unitCurvature = CommonOperation.GetUnit(UnitTypes.Curvature, "1/m");

            string[] labels = GetLabels(unitForce, unitMoment, unitCurvature);
            arrayParameter = new ArrayParameter<double>(ValidTupleList.Count(), labels.Count(), labels);
            CalculateWithoutCrack(ValidTupleList, unitForce, unitMoment, unitCurvature);
        }

        public ShowDiagramLogic(IEnumerable<IForcesTupleResult> tupleList, IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            TupleList = tupleList;
            NdmPrimitives = ndmPrimitives;
            ValidTupleList = TupleList.Where(x => x.IsValid == true).ToList();
        }

        private void CalculateWithoutCrack(List<IForcesTupleResult> resultList, IUnit unitForce, IUnit unitMoment, IUnit unitCurvature)
        {
            var data = arrayParameter.Data;
            for (int i = 0; i < resultList.Count(); i++)
            {
                var valueList = ProcessResultWithouCrack(resultList, unitForce, unitMoment, unitCurvature, i);
                for (int j = 0; j < valueList.Count; j++)
                {
                    data[i, j] = valueList[j];
                }
                SetProgress?.Invoke(i);
            }
        }


        private static List<double> ProcessResultWithouCrack(List<IForcesTupleResult> resultList, IUnit unitForce, IUnit unitMoment, IUnit unitCurvature, int i)
        {
            return new List<double>
                {
                    resultList[i].DesignForceTuple.ForceTuple.Mx * unitMoment.Multiplyer,
                    resultList[i].DesignForceTuple.ForceTuple.My * unitMoment.Multiplyer,
                    resultList[i].DesignForceTuple.ForceTuple.Nz * unitForce.Multiplyer,
                    resultList[i].LoaderResults.ForceStrainPair.StrainMatrix.Kx * unitCurvature.Multiplyer,
                    resultList[i].LoaderResults.ForceStrainPair.StrainMatrix.Ky * unitCurvature.Multiplyer,
                    resultList[i].LoaderResults.ForceStrainPair.StrainMatrix.EpsZ
                };
        }
        private static string[] GetLabels(IUnit unitForce, IUnit unitMoment, IUnit unitCurvature)
        {
            return new string[]
            {
                $"{GeometryNames.MomFstName}, {unitMoment.Name}",
                $"{GeometryNames.MomSndName}, {unitMoment.Name}",
                $"{GeometryNames.LongForceName}, {unitForce.Name}",
                $"{GeometryNames.CurvFstName}, {unitCurvature.Name}",
                $"{GeometryNames.CurvSndName}, {unitCurvature.Name}",
                $"{GeometryNames.StrainTrdName}"
            };
        }

    }
}
