using LoaderCalculator;
using LoaderCalculator.Data.Materials.MaterialBuilders;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Windows.Errors;
using StructureHelper.Windows.Graphs;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelper.Windows.ViewModels.Graphs;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelper.Windows.ViewModels.Calculations.Calculators
{
    internal class ShowDiagramLogic
    {
        static readonly CrackForceCalculator calculator = new();
        ArrayParameter<double> arrayParameter;
        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;

        public void Show(List<IForcesTupleResult> results)
        {
            var resultList = results.Where(x => x.IsValid == true).ToList();
            var unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
            var unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
            var unitCurvature = CommonOperation.GetUnit(UnitTypes.Curvature, "1/m");

            string[] labels = GetLabels(unitForce, unitMoment, unitCurvature);
            arrayParameter = new ArrayParameter<double>(resultList.Count(), labels.Count(), labels);
            CalculateWithoutCrack(resultList, unitForce, unitMoment, unitCurvature);
            SafetyProcessor.RunSafeProcess(ShowWindow, "Errors apearred during showing graph, see detailed information");
        }
        public void ShowCracks(List<IForcesTupleResult> results, IEnumerable<INdmPrimitive> ndmPrimitives)
        { 
            var resultList = results.Where(x => x.IsValid == true).ToList();
            var unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
            var unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
            var unitCurvature = CommonOperation.GetUnit(UnitTypes.Curvature, "1/m");

            string[] labels = GetCrackLabels(unitForce, unitMoment, unitCurvature);
            arrayParameter = new ArrayParameter<double>(resultList.Count(), labels.Count(), labels);
            CalculateWithCrack(resultList, ndmPrimitives, unitForce, unitMoment, unitCurvature);
            SafetyProcessor.RunSafeProcess(ShowWindow, "Errors apearred during showing graph, see detailed information");
        }

        private void CalculateWithCrack(List<IForcesTupleResult> resultList, IEnumerable<INdmPrimitive> ndmPrimitives, IUnit unitForce, IUnit unitMoment, IUnit unitCurvature)
        {
            var data = arrayParameter.Data;
            for (int i = 0; i < resultList.Count(); i++)
            {
                var valueList = new List<double>
                {
                    resultList[i].DesignForceTuple.ForceTuple.Mx * unitMoment.Multiplyer,
                    resultList[i].DesignForceTuple.ForceTuple.My * unitMoment.Multiplyer,
                    resultList[i].DesignForceTuple.ForceTuple.Nz * unitForce.Multiplyer
                };
                calculator.EndTuple = resultList[i].DesignForceTuple.ForceTuple;
                var limitState = resultList[i].DesignForceTuple.LimitState;
                var calcTerm = resultList[i].DesignForceTuple.CalcTerm;
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
            }
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
            }
        }

        private void ShowWindow()
        {
            var wnd = new GraphView(arrayParameter);
            wnd.ShowDialog();
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
        private static string[] GetCrackLabels(IUnit unitForce, IUnit unitMoment, IUnit unitCurvature)
        {
            const string crc = "CrcSofteningFactor";
            return new string[]
            {
                $"{GeometryNames.MomFstName}, {unitMoment.Name}",
                $"{GeometryNames.MomSndName}, {unitMoment.Name}",
                $"{GeometryNames.LongForceName}, {unitForce.Name}",
                $"{GeometryNames.CurvFstName}, {unitCurvature.Name}",
                $"{GeometryNames.CurvSndName}, {unitCurvature.Name}",
                $"{GeometryNames.StrainTrdName}",
                $"{crc}Ix",
                $"{crc}Iy",
                $"{crc}Az",
                $"PsiFactor"
            };
        }
    }
}
