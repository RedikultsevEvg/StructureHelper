using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    /// <summary>
    /// Create array of common values
    /// </summary>
    public class DiagramFactory
    {
        IConvertUnitLogic operationLogic = new ConvertUnitLogic();
        IGetUnitLogic unitLogic = new GetUnitLogic();
        private ArrayParameter<double> arrayParameter;
        /// <summary>
        /// Collection of force results
        /// </summary>
        public List<IExtendedForceTupleCalculatorResult> TupleResultList { get; set; }

        //public Action<int> SetProgress { get; set; }

        public ArrayParameter<double> GetCommonArray()
        {
            var labels = LabelsFactory.GetCommonLabels();
            arrayParameter = new ArrayParameter<double>(TupleResultList.Count(), labels);
            Calculate();
            return arrayParameter;
        }

        private void Calculate()
        {
            var data = arrayParameter.Data;
            for (int i = 0; i < TupleResultList.Count(); i++)
            {
                var valueList = ProcessResult(i);
                for (int j = 0; j < valueList.Count; j++)
                {
                    data[i, j] = valueList[j];
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
                    TupleResultList[i].ForcesTupleResult.ForceTuple.Mx * unitMoment.Multiplayer,
                    TupleResultList[i].ForcesTupleResult.ForceTuple.My * unitMoment.Multiplayer,
                    TupleResultList[i].ForcesTupleResult.ForceTuple.Nz * unitForce.Multiplayer,
                    TupleResultList[i].ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix.Kx * unitCurvature.Multiplayer,
                    TupleResultList[i].ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix.Ky * unitCurvature.Multiplayer,
                    TupleResultList[i].ForcesTupleResult.LoaderResults.ForceStrainPair.StrainMatrix.EpsZ
                };
        }
    }
}
