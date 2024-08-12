using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <inheritdoc/>
    public class CheckTupleCalculatorInputDataLogic : ICheckInputDataLogic<TupleCrackInputData>
    {
        const string userDataIsNull = "User crack input data is null";
        private const string CollectionOfPrimitivesIsNull = "Collection does not have any primitives";
        private string? checkResult;

        private bool result;
        /// <inheritdoc/>
        public TupleCrackInputData InputData { get; set; }


        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }


        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (InputData is null)
            {
                result = false;
                string v = ErrorStrings.ParameterIsNull + ": InputData";
                checkResult += v;
                TraceLogger?.AddMessage(v, TraceLogStatuses.Error);
                return false;
            }
            CheckPrimitives();
            CheckUserData();
            return result;
        }

        private void CheckPrimitives()
        {
            if (InputData.Primitives is null || !InputData.Primitives.Any())
            {
                result = false;
                checkResult += CollectionOfPrimitivesIsNull;
                TraceLogger?.AddMessage(CollectionOfPrimitivesIsNull, TraceLogStatuses.Error);
            }
        }

        private void CheckUserData()
        {
            if (InputData.UserCrackInputData is null)
            {
                result = false;
                checkResult += userDataIsNull;
                TraceLogger?.AddMessage(userDataIsNull, TraceLogStatuses.Error);
            }
        }
    }
}
