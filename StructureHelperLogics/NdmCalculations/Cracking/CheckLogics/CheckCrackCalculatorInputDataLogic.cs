using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic of checking of input data for crack calcultor 
    /// </summary>
    public class CheckCrackCalculatorInputDataLogic : ICheckInputDataLogic<ICrackCalculatorInputData>
    {
        private bool result;
        private ICheckPrimitiveCollectionLogic checkPrimitiveCollectionLogic;

        public ICrackCalculatorInputData InputData {  get; set; }


        public string CheckResult { get; private set; }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public CheckCrackCalculatorInputDataLogic(ICheckPrimitiveCollectionLogic checkPrimitiveCollectionLogic)
        {
            this.checkPrimitiveCollectionLogic = checkPrimitiveCollectionLogic;
        }

        public CheckCrackCalculatorInputDataLogic() : this (new CheckPrimitiveCollectionLogic())
        {
            
        }

        public bool Check()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Debug);
            result = true;
            CheckResult = string.Empty;
            CheckPrimitives();
            CheckActions();
            return result;
        }

        private void CheckPrimitives()
        {
            if (checkPrimitiveCollectionLogic is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": check primitive logic");
            }
            checkPrimitiveCollectionLogic.HasPrimitives = InputData;
            checkPrimitiveCollectionLogic.TraceLogger = TraceLogger?.GetSimilarTraceLogger();
            if (checkPrimitiveCollectionLogic.Check() == false)
            {
                result = false;
                CheckResult += checkPrimitiveCollectionLogic.CheckResult;
                TraceLogger?.AddMessage(checkPrimitiveCollectionLogic.CheckResult, TraceLogStatuses.Error);
            }
        }

        private void CheckActions()
        {
            if (InputData.ForceActions is null || (!InputData.ForceActions.Any()))
            {
                result = false;
                string message = "Calculator does not contain any actions\n";
                CheckResult += message;
                TraceLogger?.AddMessage(message, TraceLogStatuses.Error);
            };
        }


    }
}
