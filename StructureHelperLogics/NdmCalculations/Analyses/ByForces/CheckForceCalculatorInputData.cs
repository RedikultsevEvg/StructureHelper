using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    internal class CheckForceCalculatorInputData : ICheckInputDataLogic<IForceCalculatorInputData>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IAccuracy> checkAccuracyLogic;
        private ICheckEntityLogic<IHasPrimitives> checkPrimitiveCollectionLogic;

        public IForceCalculatorInputData InputData { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckForceCalculatorInputData(ICheckEntityLogic<IAccuracy> checkAccuracyLogic)
        {
            this.checkAccuracyLogic = checkAccuracyLogic;
        }

        public CheckForceCalculatorInputData() : this(new CheckAccuracyLogic())
        {
            
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (InputData is null)
            {
                string errorString = ErrorStrings.ParameterIsNull + ": input data";
                TraceMessage(errorString);
                throw new StructureHelperException(errorString);
            }
            if (InputData.Primitives is null || !InputData.Primitives.Any())
            {
                TraceMessage("Calculator does not contain any primitives");
                result = false;
            }
            if (InputData.ForceActions is null || !InputData.ForceActions.Any())
            {
                TraceMessage("Calculator does not contain any forces");
                result = false;
            }
            if (InputData.LimitStatesList is null || !InputData.LimitStatesList.Any())
            {
                TraceMessage("Calculator does not contain any limit states");
                result = false;
            }
            if (InputData.CalcTermsList is null || !InputData.CalcTermsList.Any())
            {
                TraceMessage("Calculator does not contain any calc term");
                result = false;
            }
            CheckPrimitives();
            CheckMaterials();
            CheckAccuracy();
            CheckActions();
            return result;
        }

        private void CheckMaterials()
        {
            var materials = InputData.Primitives
                .Select(x => x.NdmElement.HeadMaterial)
                .Distinct()
                .ToList();

            var checkLogic = new HeadMaterialsCheckLogic(InputData.LimitStatesList, InputData.CalcTermsList)
            {
                Entity = materials,
                TraceLogger = TraceLogger,
            };
            if (checkLogic.Check() == false)
            {
                result = false;
                checkResult += checkLogic.CheckResult;
            }
        }

        private void CheckPrimitives()
        {
            checkPrimitiveCollectionLogic ??= new HasPrimitivesCheckLogic(
                TraceLogger,
                new CheckRebarPrimitiveLogic()
                {
                    CheckRebarHostMaterial = false,
                    CheckRebarPlacement = false
                })
            { 
                Entity = InputData,
            };
            if (checkPrimitiveCollectionLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkPrimitiveCollectionLogic.CheckResult);

        }

        private void CheckActions()
        {
            var checkLogic = new CheckForceActionsLogic(TraceLogger)
            {
                Entity = InputData.ForceActions
            };
            if (checkLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkLogic.CheckResult);
        }

        private void CheckAccuracy()
        {
            checkAccuracyLogic.Entity = InputData.Accuracy;
            checkAccuracyLogic.TraceLogger = TraceLogger;
            if (checkAccuracyLogic.Check() == false)
            {
                result = false;
            }
            TraceMessage(checkAccuracyLogic.CheckResult);
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString + "\n";
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
